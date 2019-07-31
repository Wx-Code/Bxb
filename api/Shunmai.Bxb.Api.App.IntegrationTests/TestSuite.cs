using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models.Wechat;
using Shunmai.Bxb.Test.Common;
using Shunmai.Bxb.Test.Common.Models;
using Shunmai.Bxb.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.App.IntegrationTests
{
    public class TestSuite
    {
        private static BxbContext _bxbContext;

        public static BxbContext GetDbContext()
        {
            if (_bxbContext == null)
            {
                _bxbContext = BxbContext.FromSmartSqlConfig();
            }
            return _bxbContext;
        }

        public static string BuildQueryString(object data)
        {
            if (data == null) return string.Empty;
            return data.GetType()
                       .GetProperties()
                       .Where(p => p.CanRead)
                       .Select(p => $"{p.Name}={p.GetValue(data)}")
                       .Join("&");
        }

        public static async Task<T> PostAsync<T>(HttpClient client, string url, object data, Dictionary<string, string> headers = null)
        {
            var json = data == null ? string.Empty : JsonConvert.SerializeObject(data);
            client.AddHeaders(headers);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            using (var response = await client.PostAsync(url, content))
            {
                client.RemoveHeaders(headers);
                response.EnsureSuccessStatusCode();
                return await response.GetResult<T>();
            }
        }

        public static async Task<T> GetAsync<T>(HttpClient client, string url, object data = null, Dictionary<string, string> headers = null)
        {
            var query = BuildQueryString(data);
            url = $"{url}?{query}";
            client.AddHeaders(headers);
            using (var response = await client.GetAsync(url))
            {
                client.RemoveHeaders(headers);
                response.EnsureSuccessStatusCode();
                return await response.GetResult<T>();
            }
        }

        public static Mock<WechatService> GetMockWechatService()
        {
            return new Mock<WechatService>(MockBehavior.Strict, new object[] { Mock.Of<ILogger<WechatService>>(), Mock.Of<WechatConfig>() });
        }

        public static UserExt CreateTestUser()
        {
            return new UserExt
            {
                Avatar = Randoms.Letters(30),
                Nickname = Randoms.Letters(10),
                WxOpenId = Randoms.String(32),
                WxUnionId = Randoms.String(32),
                WxCodePhoto = Randoms.String(32),
                Phone = "135" + Randoms.Numbers(8),
                Realname = Randoms.Letters(10),
                CreatedTime = DateTime.Now,
                WalletAddress = Randoms.String(32),
            };
        }
    }
}
