using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shunmai.Bxb.Entities.Enums
{
    /// <summary>
    /// 货币类型
    /// </summary>
    public enum CurrencyType
    {
        /// <summary>
        /// GRT(贵人通)
        /// </summary>
        [Description("GRT(贵人通)")]
        GRT = 1,

        /// <summary>
        /// GDT(流通积分)
        /// </summary>
        [Description("GDT(流通积分)")]
        GDT = 2,

        /// <summary>
        /// VIT(VIT积分)
        /// </summary>
        [Description("VIT(VIT积分)")]
        VIT = 3,

        /// <summary>
        /// CDT(消费积分)
        /// </summary>
        [Description("CDT(消费积分)")]
        CDT = 4,
    }
}
