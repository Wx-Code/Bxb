using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shunmai.Bxb.Common.Enums
{
    public enum PurposeType
    {
        /// <summary>
        /// 转币
        /// </summary>
        [Description("转币")]
        TurnCoin = 1,

        /// <summary>
        /// 手续费
        /// </summary>
        [Description("手续费")]
        CommissionCharge = 2,
    }
}
