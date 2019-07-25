using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class TradeRecord
    {
        [JsonProperty("date")]
        public long Date { get; set; }
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("fee")]
        public string Fee { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("memos")]
        public List<MemoData> Remarks { get; set; }
        [JsonProperty("counterparty")]
        public string CounterParty { get; set; }
        [JsonProperty("amount")]
        public Amount Amount { get; set; }
    }
}
