using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Entities.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface ITradeOrderRepository
    {
        bool Insert(TradeOrder order);
        TradeOrder Find(long orderId);
        bool UpdateState(long orderId, TradeOrderState state);

        List<TradeOrderAppResponse> PageGetSellerTradeOrders(int offset, int size, int? userId, TradeOrderState? status);

        int GetSellerTradeOrdersCount(int? userId, TradeOrderState? status);

        List<TradeOrderAppResponse> PageGetBuyerTradeOrders(int offset, int size, int? userId, TradeOrderState? status);

        int GetBuyerTradeOrdersCount(int? userId, TradeOrderState? status);


        int Count(object condition);
        List<TradeOrderResponse> QueryList(object condition);

        /// <summary>
        /// 将订单状态改为确认收款，并记录收款时间。当订单状态不为“待收款”时，将返回 false.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool Confirm(long orderId);

        /// <summary>
        /// 将订单状态修改为已完成，并记录完成时间。当订单状态不为“待转币”时，将返回 false。
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool Complete(long orderId);
    }
}
