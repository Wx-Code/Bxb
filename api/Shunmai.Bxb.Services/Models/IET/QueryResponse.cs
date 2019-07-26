using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class QueryResponse
    {
        [JsonProperty("marker")]
        public Marker Marker { get; set; }
        [JsonProperty("data")]
        public List<TradeRecord> Data { get; set; }
    }
}
