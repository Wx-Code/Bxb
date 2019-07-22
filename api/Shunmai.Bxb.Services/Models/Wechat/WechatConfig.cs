using Shunmai.Bxb.Utilities.Extenssions;
using Shunmai.Bxb.Utilities.Helpers;

namespace Shunmai.Bxb.Services.Models.Wechat
{
    public class WechatConfig
    {
        public string AppId { get; set; }
        public string AppSecrect { get; set; }
        public string ApiKey { get; set; }
        public string Token { get; set; }
        public string MerchantId { get; set; }
        public string CertPath { get; set; }
        public string CertAbsolutePath => CertPath.IsEmpty() ? string.Empty : PathHelper.MapPath(CertPath);
        public string CertPassword { get; set; }
        public string RefundNotifyUrl { get; set; }
        public string NotifyUrl { get; set; }
    }
}
