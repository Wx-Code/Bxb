using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
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
        public static string BuildQueryString(object data)
        {
            if (data == null) return string.Empty;
            return data.GetType()
                       .GetProperties()
                       .Where(p => p.CanRead)
                       .Select(p => $"{p.Name}={p.GetValue(data)}")
                       .Join("&");
        }

        public static async Task<T> PostAsync<T>(WebApplicationFactory<Startup> factory, string url, object json)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json"))
            using (var client = factory.CreateClient())
            using (var response = await client.PostAsync(url, content))
            {
                response.EnsureSuccessStatusCode();
                return await response.GetResult<T>();
            }
        }

        public static async Task<T> GetAsync<T>(WebApplicationFactory<Startup> factory, string url, object json)
        {
            var query = BuildQueryString(json);
            url = $"{url}?{query}";
            using (var client = factory.CreateClient())
            using (var response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                return await response.GetResult<T>();
            }
        }
    }
}
