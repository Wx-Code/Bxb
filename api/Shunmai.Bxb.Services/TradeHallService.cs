using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Entities.Views;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Attributes;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Services.Utils;
using Shunmai.Bxb.Utilities.Check;
using System.Collections.Generic;
using Shunmai.Bxb.Utilities.Extenssions;

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
                OperateLog = $"用户编号：{entity.ReleaseUserId.ToString()} 发布新的交易信息。币种：{entity.BType.ToString()}, 总数量：{entity.TotalAmount}, 单价：{entity.Price} "
            };

            log.LogId = _tradeHallLogRepository.InsertTradeHallLogEntity(log);

            return log.LogId <= 0 ? (500, "添加交易日志信息失败") : (201, "添加成功");
        }

        [SmartSqlTransaction]
        public (int, string) UpdateTradeHallEntity(TradeHall entity)
        {
            Check.EnsureMoreThanZero(entity.TradeId, nameof(entity.TradeId));

            entity.State = TradeHallState.Working;
            int rowCount = _tradeHallRepository.UpdateTradeHallEntity(entity);

            if(rowCount != 1) return (500, "修改交易信息失败");

            TradeHallLog log = new TradeHallLog
            {
                OperateId = entity.ReleaseUserId,
                OperateName = entity.ReleaseName,
                TradeHallId = entity.TradeId,
                OperateLog = $"用户编号：{entity.ReleaseUserId.ToString()} 更新的交易信息。交易编号：{entity.TradeId.ToString()}，币种：{entity.BType.ToString()}, 总数量：{entity.TotalAmount}, 单价：{entity.Price} "
            };

            log.LogId = _tradeHallLogRepository.InsertTradeHallLogEntity(log);

            return log.LogId <= 0 ? (500, "添加交易日志信息失败") : (200, "修改成功");
        }

        [SmartSqlTransaction]
        public (int, string) UpdateTradeHallStatus(int tradeId, TradeHallShelfStatus status, int userId, string userNickName)
        {
            Check.EnsureMoreThanZero(tradeId, nameof(tradeId));

            int rowCount = _tradeHallRepository.UpdateTradeHallStatus(status, tradeId);

            if (rowCount != 1) return (500, "修改交易信息失败");

            TradeHallLog log = new TradeHallLog
            {
                OperateId = userId,
                OperateName = userNickName,
                TradeHallId = tradeId,
                OperateLog = $"用户编号：{userId.ToString()} 更新的交易信息状态。交易编号：{tradeId.ToString()}，状态：{status.GetDescription()}"
            };

            log.LogId = _tradeHallLogRepository.InsertTradeHallLogEntity(log);

            return log.LogId <= 0 ? (500, "添加交易日志信息失败") : (200, "修改成功");
        }

        public (int num, List<TradeHallAppResponse> result) PagedGetAppTradeHalls(Pager query)
        {
            List<TradeHallAppResponse> result = _tradeHallRepository.PagedGetAppTradeHalls(query.Offset, query.Size);

            int count = _tradeHallRepository.GetAppTradeHallsCount();

            int num = count % query.Size == 0 ? count / query.Size : count / query.Size + 1;

            return (num, result);
        }

        public TradeHallAppResponse GetAppTradeHallDetail(int tradeId)
        {
            return _tradeHallRepository.GetAppTradeHallDetail(tradeId);
        }

        public TradeHall GetSingleTradeHallEntity(int tradeId)
        {
            Check.EnsureMoreThanZero(tradeId, nameof(tradeId));

            return _tradeHallRepository.GetSingleTradeHallEntity(tradeId);
        }

        public (int count, List<TradeHallAppResponse> result) PageGetAdminTradeHalls(TradeHallQuery query)
        {
            List<TradeHallAppResponse> result = _tradeHallRepository.PageGetAdminTradeHalls(query.Offset, query.Size, query.UserId, query.BType, query.Status, query.StartTime, query.EndTime);

            int count = _tradeHallRepository.GetAdminTradeHallsCount(query.UserId, query.BType, query.Status, query.StartTime, query.EndTime);

            return (count, result);
        }
    }
}
