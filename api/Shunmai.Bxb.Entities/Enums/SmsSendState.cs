﻿using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    public enum SmsSendState
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
