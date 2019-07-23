using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shunmai.Bxb.Entities.Enums
{
    /// <summary>
    /// 短信服务端
    /// </summary>
    public enum SmsProvider
    {
        /// <summary>
        /// 当前
        /// </summary>
        [Description("当前")]
        Current = 0,

        /// <summary>
        /// 阿里
        /// </summary>
        [Description("阿里")]
        Alibaba = 1,
    }
}
