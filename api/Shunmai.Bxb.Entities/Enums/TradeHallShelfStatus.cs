using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    /// <summary>
    /// 交易大厅数据状态
    /// </summary>
    public enum TradeHallShelfStatus
    {
        /// <summary>
        /// 下架
        /// </summary>
        [Description("下架")]
        Off = 0,

        /// <summary>
        /// 上架
        /// </summary>
        [Description("上架")]
        On = 1,

        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Deleted = -1,
    }
}
