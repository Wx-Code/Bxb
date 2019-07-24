using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shunmai.Bxb.Entities.Enums
{
    public enum SmsCodeState
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 已验证
        /// </summary>
        [Description("已验证")]
        Verified = 1,

        /// <summary>
        /// 已过期
        /// </summary>
        [Description("已过期")]
        Expired = -1,
    }
}
