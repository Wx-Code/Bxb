using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.App.IntegrationTests
{
    public static class Extensions
    {
        public static async Task<T> GetResult<T>(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Join(this IEnumerable<string> strs, string seperator)
        {
            return string.Join(seperator, strs.ToArray());
        }

        public static T GetService<T>(this WebApplicationFactory<Startup> factory) where T: class
        {
            return factory.Server.Host.Services.GetService(typeof(T)) as T;
        }

        public static T GetService<T>(this TestServer server) where T : class
        {
            return server.Host.Services.GetService(typeof(T)) as T;
        }

        public static void AddHeaders(this HttpClient client, IDictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, headers[key]);
                }
            }
        }

        public static void RemoveHeaders(this HttpClient client, IDictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    client.DefaultRequestHeaders.Remove(key);
                }
            }
        }

        public static async Task<T> PostAsync<T>(this HttpClient client, string url, object data, Dictionary<string, string> headers = null)
        {
            return await TestSuite.PostAsync<T>(client, url, data, headers);
        }

        public static async Task<T> GetAsync<T>(this HttpClient client, string url, object data = null, Dictionary<string, string> headers = null)
        {
            return await TestSuite.GetAsync<T>(client, url, data, headers);
        }
    }
}
