using System;
using System.Collections.Generic;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Entities.Views;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Attributes;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Services.Utils;
using Shunmai.Bxb.Utilities.Validation;

namespace Shunmai.Bxb.Services
{
    public class TradeHallService
    {
        private readonly ITradeHallRepository _tradeHallRepository;
        private readonly ITradeHallLogRepository _tradeHallLogRepository;

        public TradeHallService(ITradeHallRepository tradeHallRepository, ITradeHallLogRepository tradeHallLogRepository)
        {
            _tradeHallRepository = tradeHallRepository;
            _tradeHallLogRepository = tradeHallLogRepository;
        }

        [SmartSqlTransaction]
        public (int, string) InsertTradeHallEntity(TradeHall entity)
        {
            entity.TradeType = TradeType.Selling;
            entity.Amount = entity.TotalAmount;
            entity.State = TradeHallState.Working;
            entity.Status = TradeHallShelfStatus.On;
            entity.TradeCode = IdGenerator.GenerateTradeCode();
            entity.TradeId = _tradeHallRepository.InsertTradeHallEntity(entity);

            if (entity.TradeId <= 0) return (500, "添加交易信息失败");

            TradeHallLog log = new TradeHallLog
            {
                OperateId = entity.ReleaseUserId,
                OperateName = entity.ReleaseName,
                TradeHallId = entity.TradeId,
                OperateLog = $"用户编号：{entity.ReleaseUserId} 发布新的交易信息。币种：{entity.BType.ToString()}, 总数量：{entity.TotalAmount}, 单价：{entity.Price} "
            };

            log.LogId = _tradeHallLogRepository.InsertTradeHallLogEntity(log);

            return log.LogId <= 0 ? (500, "添加交易日志信息失败") : (201, "添加成功");
        }

        public (int, string) UpdateTradeHallEntity(TradeHall entity)
        {
            Check.EnsureGreaterThanZero(entity.TradeId, nameof(entity.TradeId));

            entity.State = TradeHallState.Working;
            int rowCount = _tradeHallRepository.UpdateTradeHallEntity(entity);

            return rowCount != 1 ? (500, "修改交易信息失败") : (200, "修改成功");
        }

        public (int num, List<TradeHallAppResponse> result) PagedGetAppTradeHalls(Pager query, int? userId)
        {
            List<TradeHallAppResponse> result = _tradeHallRepository.PagedGetAppTradeHalls(query.Offset, query.Size, userId);

            long count = _tradeHallRepository.GetAppTradeHallsCount();

            int num = count % query.Size == 0 ? Convert.ToInt32(count / query.Size) : Convert.ToInt32(count / query.Size + 1);

            return (num, result);
        }

        public TradeHallAppResponse GetAppTradeHallDetail(int tradeId)
        {
            return _tradeHallRepository.GetAppTradeHallDetail(tradeId);
        }

        public TradeHall GetSingleTradeHallEntity(int tradeId)
        {
            Check.EnsureGreaterThanZero(tradeId, nameof(tradeId));

            return _tradeHallRepository.GetSingleTradeHallEntity(tradeId);
        }
    }
}
