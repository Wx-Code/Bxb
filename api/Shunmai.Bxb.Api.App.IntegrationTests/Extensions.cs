using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    }
}
