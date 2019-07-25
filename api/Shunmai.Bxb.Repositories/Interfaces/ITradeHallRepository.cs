using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Entities.Views;
using System.Collections.Generic;

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
        /// <returns>受影响行数</returns>
        int UpdateTradeHallStatus(TradeHallShelfStatus status, int tradeId);

        /// <summary>
        /// 分页获取APP首页交易大厅的交易信息
        /// </summary>
        /// <param name="offset">偏移量</param>
        /// <param name="size">每页条数</param>
        /// <returns>当前页交易大厅中的交易信息</returns>
        List<TradeHallAppResponse> PagedGetAppTradeHalls(int offset, int size);

        /// <summary>
        /// 获取APP首页交易大厅的交易信息总条数
        /// </summary>
        /// <returns>交易信息总条数</returns>
        int GetAppTradeHallsCount();

        /// <summary>
        /// 获取APP交易信息详情
        /// </summary>
        /// <param name="tradeId">交易信息ID</param>
        /// <returns>交易信息详情</returns>
        TradeHallAppResponse GetAppTradeHallDetail(int tradeId);

        /// <summary>
        /// 获取单条交易信息数据
        /// </summary>
        /// <param name="tradeId">交易信息ID</param>
        /// <returns>单条交易信息数据</returns>
        TradeHall GetSingleTradeHallEntity(int tradeId);

        /// <summary>
        /// 分页获取用户发布的交易信息
        /// </summary>
        /// <param name="offset">偏移量</param>
        /// <param name="size">每页条数</param>
        /// <param name="userId">用户ID</param>
        /// <returns>当前页用户发布的交易信息</returns>
        List<TradeHallAppResponse> PagedGetAppUserPublishTradeHalls(int offset, int size, int userId);

        /// <summary>
        /// 获取用户发布的交易信息总体条数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户发布的交易信息总体条数</returns>
        int GetAppUserPublishTradeHallsCount(int userId);
    }
}