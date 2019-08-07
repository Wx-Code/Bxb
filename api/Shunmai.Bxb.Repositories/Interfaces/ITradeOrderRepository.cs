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
    }
}
