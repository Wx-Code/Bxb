using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    public enum SmsType
    {
        /// <summary>
        /// 验证码
        /// </summary>
        [Description("验证码")]
        Verification = 0,

        /// <summary>
        /// 通知
        /// </summary>
        [Description("通知")]
        Notification = 1,
    }
}
