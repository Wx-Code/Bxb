using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
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
    }
}
