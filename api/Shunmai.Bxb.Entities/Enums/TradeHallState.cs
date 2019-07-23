using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    public enum TradeHallState
    {
        /// <summary>
        /// 正常进行中
        /// </summary>
        [Description("正常进行中")]
        Working = 0,

        /// <summary>
        /// 已完成交易
        /// </summary>
        [Description("已完成交易")]
        Completed = 1,
    }
}
