using Newtonsoft.Json;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class QueryRequest
    {
        [JsonProperty("market")]
        public string Market { get; set; }
        [JsonProperty("walletid")]
        public string WalletId { get; set; }
    }
}
