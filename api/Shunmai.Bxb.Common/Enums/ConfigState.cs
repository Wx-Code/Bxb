using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shunmai.Bxb.Common.Enums
{
    public enum ConfigState
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disabled = 1,
    }
}
