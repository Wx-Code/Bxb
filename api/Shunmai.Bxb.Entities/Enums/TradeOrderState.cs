using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    /// <summary>
    /// 交易订单状态
    /// </summary>
    public enum TradeOrderState
    {
        /// <summary>
        /// 待转币订单（待卖家转币给平台）
        /// </summary>
        [Description("等待卖家转币")]
        SellerOperating = 0,

        /// <summary>
        /// 待收款订单（平台已确认收到卖家转币）
        /// </summary>
        [Description("等待买家付款")]
        BuyerPaying = 10,

        /// <summary>
        /// 待平台转币订单（卖家已确认收款，等待平台转币给买家）
        /// </summary>
        [Description("等待平台转币")]
        PlatformOperating = 20,

        /// <summary>
        /// 已完成订单（平台转币给买家，订单完成）
        /// </summary>
        [Description("交易完成")]
        Completed = 30,

        /// <summary>
        /// 已取消订单（未转币到平台的订单，买家操作了取消订单等）
        /// </summary>
        [Description("订单取消")]
        Canceled = -1,
    }
}
