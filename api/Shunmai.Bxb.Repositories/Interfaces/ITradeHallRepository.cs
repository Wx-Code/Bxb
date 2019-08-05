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
        /// <param name="bType">货币类型</param>
        /// <param name="offset">偏移量</param>
        /// <param name="size">每页条数</param>
        /// <returns>当前页交易大厅中的交易信息</returns>
        List<TradeHallAppResponse> PagedGetAppTradeHalls(CurrencyType? bType, int offset, int size);

        /// <summary>
        /// 获取APP首页交易大厅的交易信息总条数
        /// </summary>
        /// <param name="bType">货币类型</param>
        /// <returns>交易信息总条数</returns>
        int GetAppTradeHallsCount(CurrencyType? bType);

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
        /// 后台分页获取交易信息
        /// </summary>
        /// <param name="offset">偏移量</param>
        /// <param name="size">每页条数</param>
        /// <param name="userId">用户ID</param>
        /// <param name="bType">货币类型</param>
        /// <param name="status">状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        List<TradeHallAppResponse> PageGetAdminTradeHalls(int offset, int size, int? userId, CurrencyType? bType, TradeHallShelfStatus? status, string startTime, string endTime);

        /// <summary>
        /// 后台获取交易信息总条数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bType">货币类型</param>
        /// <param name="status">状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>用户发布的交易信息总条数</returns>
        int GetAdminTradeHallsCount(int? userId, CurrencyType? bType, TradeHallShelfStatus? status, string startTime, string endTime);

        /// <summary>
        /// 更新可交易数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        bool UpdateAmount(int id, decimal amount);

        TradeHall Find(int id);
        bool UpdateState(int id, TradeHallState state);
    }
}