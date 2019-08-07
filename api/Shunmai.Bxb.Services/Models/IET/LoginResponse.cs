using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class LoginResponse
    {
        [JsonProperty("rongyunToken")]
        public string RongyunToken { get; set; }
        [JsonProperty("repalceFlag")]
        public int ReplaceFlag { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("custId")]
        public string CusId { get; set; }
        [JsonProperty("account")]
        public Account Account { get; set; }
        [JsonIgnore]
        public string Token { get; set; }
    }

    public class Account
    {
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
        [JsonProperty("account")]
        public string AccountString { get; set; }
    }
}
