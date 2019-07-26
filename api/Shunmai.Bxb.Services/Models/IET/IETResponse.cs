using Newtonsoft.Json;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class IETResponse<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
