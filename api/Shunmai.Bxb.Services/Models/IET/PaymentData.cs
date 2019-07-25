using Newtonsoft.Json;

namespace Shunmai.Bxb.Services.Models.IET
{
    /// <summary>
    /// IET 交易接口所需的数据模型
    /// </summary>
    public class PaymentData
    {
        [JsonProperty("currency")]
        public string Currency { get; set; } = "GRT";
        [JsonProperty("destAddress")]
        public string DestAddress { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        [JsonProperty("memo")]
        public string Remark { get; set; }
        [JsonProperty("walletId")]
        public string WalletId { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("passwd")]
        public string Password { get; set; }
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; } = "86";
    }
}
