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
    }
}
