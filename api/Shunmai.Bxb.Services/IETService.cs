using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Services.Exceptions;
using Shunmai.Bxb.Services.Models.IET;
using Shunmai.Bxb.Utilities.Check;
using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Services
{
    /// <summary>
    /// 处理 IET 相关的业务逻辑
    /// </summary>
    public class IETService
    {
        private const string API_BASE = "https://gdt.huaxuec.com";
        private const string LOGIN_URL = API_BASE + "/customer/login.json";
        private const string PAY_API_URL = API_BASE + "/customer/api/payment.json";
        private const string QUERY_API_URL = API_BASE + "/customer/api/getpayments.json";
        private const string HOST = "gdt.huaxuec.com";
        private const string JSON_MIME_TYPE = "application/json";
        private const SecurityProtocolType HTTPS_PROTOCOL_TYPE = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        private const int MAX_RETRY_CNT = 5;
        private const string COOKIE_NAME_OF_LOGIN_TOKEN = "WEBID";
        private const string CACHE_PREFIX = "IETService:";
        private const int CACHE_EXPIRE_SECONDS = 30 * 24 * 3600;

        private readonly ICache _cache;
        private readonly ILogger _logger;

        public IETService(ILogger<IETService> logger, ICache cache)
        {
            _cache = cache;
            _logger = logger;
        }

        private async Task<T> PostAsync<T>(string url, string json, Dictionary<string, string> headers = null)
        {
            using (var content = new StringContent(json, Encoding.UTF8, JSON_MIME_TYPE))
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = HTTPS_PROTOCOL_TYPE;
                if (headers != null)
                {
                    foreach (var key in headers.Keys)
                    {
                        client.DefaultRequestHeaders.Add(key, headers[key]);
                    }
                }

                using (var response = await client.PostAsync(url, content))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
            }
        }

        private string Serialize(object data)
        {
            var json = data.JsonSerialize();
            return json.Replace(@"\\", @"\");
        }

        private Dictionary<string, string> BuildIETRequestHeaders(string loginToken = null)
        {
            return new Dictionary<string, string>
            {
                { "Host", HOST },
                { "Cookie", $"{COOKIE_NAME_OF_LOGIN_TOKEN}={loginToken}" },
            };
        }

        public async Task<bool> PayAsync(IETWallet wallet, string destAddress, decimal amount, string remark)
        {
            var loginInfo = await GetEffctiveLoginInfoAsync(wallet.Phone, wallet.LoginPassword);
            var postData = new PaymentData
            {
                Amount = amount,
                DestAddress = destAddress,
                Password = wallet.TradePassword,
                Phone = wallet.Phone,
                Remark = remark,
                WalletId = wallet.WalletId,
            };
            var headers = BuildIETRequestHeaders(loginInfo.Token);
            var response = await PostAsync<IETResponse<string>>(PAY_API_URL, Serialize(postData), headers);
            if (response.Success == false)
            {
                _logger.LogError($"Failed to pay to the specified wallet. Response: {response.ToLogFormatString()}");
            }
            return response.Success;
        }

        public async Task<IETResponse<QueryResponse>> QueryTradeRecordsAsync(IETWallet wallet, long? ledger = null, int? seq = null)
        {
            Check.Null(wallet, nameof(wallet));
            var marker = (ledger == null || seq == null) ? null : new Marker(ledger.Value, seq.Value);
            var loginInfo = await GetEffctiveLoginInfoAsync(wallet.Phone, wallet.LoginPassword);
            var headers = BuildIETRequestHeaders(loginInfo.Token);
            var query = new QueryRequest(wallet.WalletId, marker);
            return await PostAsync<IETResponse<QueryResponse>>(QUERY_API_URL, Serialize(query), headers);
        }

        private string GetCacheKey(string key)
        {
            return $"{CACHE_PREFIX}{key}";
        }

        /// <summary>
        /// 通过执行一次交易记录查询，来确定给定的登录信息是否有效
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        private async Task<bool> IsLoginInfoEffective(LoginResponse loginInfo)
        {
            var query = new QueryRequest(loginInfo.CusId, null);
            var headers = BuildIETRequestHeaders(loginInfo.Token);
            var response = await PostAsync<IETResponse<QueryResponse>>(QUERY_API_URL, Serialize(query), headers);
            return response.Success;
        }

        /// <summary>
        /// 获取有效的登录信息。此方法将尝试从缓存中获取登录信息，并验证登录信息是否有效。
        /// 若失败，则会尝试重新登录以获取新的登录信息。
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<LoginResponse> GetEffctiveLoginInfoAsync(string account, string password)
        {
            var key = GetCacheKey(account);
            var loginInfo = _cache.Get<LoginResponse>(key);
            if (loginInfo != null)
            {
                // 判断当前缓存的登录信息是否依然有效
                var effective = await IsLoginInfoEffective(loginInfo);
                if (effective)
                {
                    return loginInfo;
                }
            }

            loginInfo = await LoginForTokenAsync(account, password);
            if (loginInfo.Token.IsEmpty())
            {
                throw new IETLoginException("Cannot get token after logging in.");
            }

            if (_cache.Set(key, loginInfo, TimeSpan.FromSeconds(CACHE_EXPIRE_SECONDS)) == false)
            {
                throw new CacheException();
            }
            return loginInfo;
        }

        private async Task<LoginResponse> LoginForTokenAsync(string account, string loginPwd)
        {
            Check.Empty(account, nameof(account));
            Check.Empty(loginPwd, nameof(loginPwd));

            var retryCnt = 1;
            do
            {
                var loginRes = await LoginAsync(new LoginRequest { Acount = account, Password = loginPwd });
                if (loginRes.Success)
                {
                    return loginRes.Data;
                }
                if (loginRes.Code == ApiCode.PWD_ERROR || loginRes.Code == ApiCode.PWD_EXCEPTION)
                {
                    throw new IETLoginException("Password error detected on logging in IET.");
                }
                _logger.LogError($"Login failed for the account {account}. Retry: {retryCnt}, Response: {loginRes.ToLogFormatString()}");

            } while (retryCnt++ < MAX_RETRY_CNT);

            throw new IETLoginException($"Failed to log in.");
        }

        private async Task<IETResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var content = new StringContent(Serialize(request), Encoding.UTF8, JSON_MIME_TYPE))
            using (var client = new HttpClient(handler))
            {
                ServicePointManager.SecurityProtocol = HTTPS_PROTOCOL_TYPE;
                var headers = BuildIETRequestHeaders();
                foreach (var key in headers.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, headers[key]);
                }
                using (var response = await client.PostAsync(LOGIN_URL, content))
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var loginRes = JsonConvert.DeserializeObject<IETResponse<LoginResponse>>(json);
                    var cookies = cookieContainer.GetCookies(new Uri(API_BASE)).Cast<Cookie>();
                    var token = cookies.FirstOrDefault(c => c.Name == COOKIE_NAME_OF_LOGIN_TOKEN)?.Value;
                    if (loginRes.Data != null) loginRes.Data.Token = token;
                    return loginRes;
                }
            }
        }
    }
}
