using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class Marker
    {
        [JsonProperty("ledger")]
        public long Ledger { get; set; }
        [JsonProperty("seq")]
        public int Seq { get; set; }

        public Marker(long ledger, int seq)
        {
            Ledger = ledger;
            Seq = seq;
        }
    }
}
