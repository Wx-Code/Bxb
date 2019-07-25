using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class MemoData
    {
        [JsonProperty("MemoData")]
        public string Remark { get; set; }
    }
}
