﻿using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Attributes;
using Shunmai.Bxb.Services.Utils;

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
            entity.State = TradeHallState.Working;
            int rowCount = _tradeHallRepository.UpdateTradeHallEntity(entity);

            return rowCount != 1 ? (500, "修改交易信息失败") : (200, "修改成功");
        }
    }
}
