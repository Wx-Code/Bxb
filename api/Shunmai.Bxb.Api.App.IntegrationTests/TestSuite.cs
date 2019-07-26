using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models.Wechat;
using Shunmai.Bxb.Test.Common;
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

        public static async Task<T> PostAsync<T>(HttpClient client, string url, object json)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json"))
            using (var response = await client.PostAsync(url, content))
            {
                response.EnsureSuccessStatusCode();
                return await response.GetResult<T>();
            }
        }

        public static async Task<T> GetAsync<T>(HttpClient client, string url, object json)
        {
            var query = BuildQueryString(json);
            url = $"{url}?{query}";
            using (var response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                return await response.GetResult<T>();
            }
        }

        public static Mock<WechatService> GetMockWechatService()
        {
            return new Mock<WechatService>(MockBehavior.Strict, new object[] { Mock.Of<ILogger<WechatService>>(), Mock.Of<WechatConfig>() });
        }
    }
}
