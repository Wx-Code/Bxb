using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    public enum TradeType
    {
        /// <summary>
        /// 卖出
        /// </summary>
        [Description("卖出")]
        Selling = 0,

        /// <summary>
        /// 购入
        /// </summary>
        [Description("购入")]
        Buying = 1,
    }
}
