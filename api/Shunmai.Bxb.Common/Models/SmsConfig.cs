namespace Shunmai.Bxb.Common.Models
{
    /// <summary>
    /// 短信配置
    /// </summary>
    public class SmsConfig
    {
        public string SmsProvider { get; set; }
        public int VerificationCodeLength { get; set; }
        public int ExpiresMinutes { get; set; }
    }
}
