using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    /// <summary>
    ///  平台钱包地址 用途 枚举
    /// </summary>
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
