using Newtonsoft.Json;
using Shunmai.Bxb.Services.Models.IET;
using Shunmai.Bxb.Utilities.Extenssions;
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
        private const string PAY_API_URL = "https://gdt.huaxuec.com/customer/api/payment.json";
        private const string QUERY_API_URL = "https://gdt.huaxuec.com/customer/api/getpayments.json";
        private const string HOST = "gdt.huaxuec.com";

        private readonly IETConfig _config;

        public IETService(IETConfig config)
        {
            _config = config;
        }

        private async Task<T> Post<T>(string url, string json, string cookie)
        {
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.DefaultRequestHeaders.Add("Host", HOST);
                client.DefaultRequestHeaders.Add("Cookie", cookie);
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

        public async Task<IETResponse<string>> PayAsync(string destAddress, decimal amount, string remark)
         {
            var postData = new PaymentData
            {
                Amount = amount,
                DestAddress = destAddress,
                Password = _config.Password,
                Phone = _config.Phone,
                Remark = remark,
                WalletId = _config.WalletId,
            };
            return await Post<IETResponse<string>>(PAY_API_URL, Serialize(postData), _config.Cookie);
        }

        public async Task<IETResponse<QueryResponse>> QueryTradeRecordsAsync()
        {
            return await QueryTradeRecordsAsync(null, null);
        }

        public async Task<IETResponse<QueryResponse>> QueryTradeRecordsAsync(long? ledger, int? seq)
        {
            var marker = (ledger == null || seq == null) ? null : new Marker(ledger.Value, seq.Value);
            var query = new QueryRequest(_config.WalletId, marker);
            return await Post<IETResponse<QueryResponse>>(QUERY_API_URL, Serialize(query), _config.Cookie);
        }
    }
}
