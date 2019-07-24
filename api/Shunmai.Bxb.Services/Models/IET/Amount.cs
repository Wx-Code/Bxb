using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class Amount
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
    }
}
