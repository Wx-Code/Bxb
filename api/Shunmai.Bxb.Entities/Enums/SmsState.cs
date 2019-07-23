using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shunmai.Bxb.Entities.Enums
{
    public enum SmsState
    {
        /// <summary>
        /// 发送失败
        /// </summary>
        [Description("发送失败")]
        Failed = 0,

        /// <summary>
        /// 发送成功
        /// </summary>
        [Description("发送成功")]
        Succeed = 1,
    }
}
