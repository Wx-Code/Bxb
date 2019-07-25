using Newtonsoft.Json;
using Shunmai.Bxb.Utilities.Extenssions;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class QueryRequest
    {
        [JsonProperty("market", NullValueHandling = NullValueHandling.Ignore)]
        public string Market { get; private set; }
        [JsonProperty("walletid")]
        public string WalletId { get; private set; }
        [JsonIgnore]
        public Marker MarkerObject { get; private set; }
        [JsonProperty("marker", NullValueHandling = NullValueHandling.Ignore)]
        public string Marker => MarkerObject == null ? null : MarkerObject.JsonSerialize();

        public QueryRequest(string walletId, Marker marker, string market = null)
        {
            WalletId = walletId;
            MarkerObject = marker;
            Market = market;
        }
    }
}
