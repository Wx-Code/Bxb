using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface ITradeHallRepository
    {
        /// <summary>
        /// 新增交易信息
        /// </summary>
        /// <param name="entity">交易信息</param>
        /// <returns>交易信息ID</returns>
        int InsertTradeHallEntity(TradeHall entity);

        /// <summary>
        /// 修改交易信息（币种类型、交易金额、单价）
        /// </summary>
        /// <param name="entity">币种类型、交易金额、单价 信息</param>
        /// <returns>受影响行数</returns>
        int UpdateTradeHallEntity(TradeHall entity);

        /// <summary>
        /// 修改交易的上下架状态
        /// </summary>
        /// <param name="status">上下架状态</param>
        /// <param name="tradeId">交易ID</param>
        /// <returns></returns>
        int UpdateTradeHallStatus(TradeHallShelfStatus status, int tradeId);
    }
}