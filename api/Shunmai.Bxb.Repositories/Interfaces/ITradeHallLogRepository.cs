using Shunmai.Bxb.Entities;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface ITradeHallLogRepository
    {
        /// <summary>
        /// 添加交易日志
        /// </summary>
        /// <param name="entity">日志信息</param>
        /// <returns>交易日志ID</returns>
        int InsertTradeHallLogEntity(TradeHallLog entity);
    }
}