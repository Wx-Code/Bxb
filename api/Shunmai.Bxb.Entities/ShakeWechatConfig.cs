using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Entities
{
    public class ShakeWechatConfig
    {
        public string AppId { get; set; }
        public string AppSecrect { get; set; }

        public string NotifyUrl { get; set; }

        public string WechatMerchantId { get; set; }

        public string WechatMerchantKey { get; set; }

        public string Cert { get; set; }
        public string CertPassword { get; set; }

        public string RefundNotifyUrl { get; set; }
    }


}
