using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Attributes;
using Shunmai.Bxb.Services.Enums;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Services.Utils;
using Shunmai.Bxb.Utilities.Check;
using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services
{
    public class TradeOrderService
    {
        private readonly ILogger _logger;
        private readonly ITradeHallRepository _hallRepos;
        private readonly ITradeOrderRepository _orderRepos;
        private readonly ITradeOrderLogRepository _orderLogRepos;

        public TradeOrderService(
            ILogger<TradeOrderService> logger
            , ITradeHallRepository hallRepository
            , ITradeOrderRepository orderRepository
            , ITradeOrderLogRepository logRepository
        )
        {
            _logger = logger;
            _hallRepos = hallRepository;
            _orderRepos = orderRepository;
            _orderLogRepos = logRepository;
        }

        private TradeOrder CreateOrder(SubmitData data)
        {
            var hall = data.Hall;
            return new TradeOrder
            {
                OrderId = IdGenerator.Generate(IdGenerator.IdType.OrderId),
                Amount = data.RequiredCount,
                Btype = hall.BType,
                BuyerPhone = data.Buyer.Phone,
                BuyerUserId = data.Buyer.UserId,
                BuyerWalletAddress = data.Buyer.WalletAddress,
                CreateTime = DateTime.Now,
                PlatServiceWalletAddress = data.ServiceFeeReceiveWalletAddr,
                PlatWalletAddress = data.PlatformWalletAddr,
                Price = hall.Price,
                TotalAmount = hall.Price * data.RequiredCount,
                SellerPhone = data.Seller.Phone,
                SellerUserId = data.Seller.UserId,
                SellerWalletAddress = data.Seller.WalletAddress,
                TradeCode = hall.TradeCode,
                ServiceAmount = data.ServiceFee,
                State = TradeOrderState.SellerOperating,
                TradeType = TradeType.Selling,
                TradeId = hall.TradeId,
            };
        }

        private bool CanSubmit(SubmitData data, out OrderSubmitResult result)
        {
            var hall = data.Hall;
            if (hall.Status != TradeHallShelfStatus.On)
            {
                result = OrderSubmitResult.TradeHallStatusException;
                return false;
            }
            if (hall.State != TradeHallState.Working)
            {
                result = OrderSubmitResult.TradeHallStateException;
                return false;
            }
            if (hall.TradeCode != data.TradeCode)
            {
                result = OrderSubmitResult.TradeCodeInputError;
                return false;
            }
            if (hall.Amount < data.RequiredCount)
            {
                result = OrderSubmitResult.CountNotEnough;
                return false;
            }

            result = OrderSubmitResult.Success;
            return true;
        }

        /// <summary>
        /// 尝试提交订单，如果出现如下情形之一，则返回 false 表示订单提交失败
        ///     1. 如果交易信息不存在
        ///     2. 如果交易信息的 `Status` 不处于上架状态
        ///     3. 如果交易信息的 `State` 不处于正常进行中
        ///     4. 如果输入的交易码错误
        ///     5. 如果输入的购买数量大于剩余数量
        ///     6. 尝试创建交易订单，如果创建失败
        ///     7. 尝试创建交易日志，如果创建失败
        ///     8. 尝试更新可交易数量，如果更新失败
        /// </summary>
        /// <param name="hallId"></param>
        /// <param name="count"></param>
        /// <param name="tradeCode"></param>
        /// <param name="buyerId"></param>
        /// <returns></returns>
        [SmartSqlTransaction]
        public bool Submit(SubmitData data, out OrderSubmitResult result)
        {
            if (CanSubmit(data, out result) == false)
            {
                return false;
            }

            var order = CreateOrder(data);
            var addOrderSuccess = _orderRepos.Insert(order);
            if (addOrderSuccess == false)
            {
                result = OrderSubmitResult.PersistenceFailed;
                _logger.LogError($"Insert into `TradeOrder` failed.");
                return false;
            }

            var orderLog = new TradeOrderLog
            {
                OperateId = data.Buyer.UserId,
                OperateLog = "用户下单",
                OperateName = data.Buyer.Nickname,
                OrderId = order.OrderId,
            };
            var logId = _orderLogRepos.Insert(orderLog);
            if (logId <= 0)
            {
                result = OrderSubmitResult.PersistenceFailed;
                _logger.LogError($"Insert into `TradeOrderLog` failed.");
                return false;
            }
            // 更新可交易数量
            var restAmount = data.Hall.Amount - data.RequiredCount;
            var updateSuccess = _hallRepos.UpdateAmount(data.Hall.TradeId, restAmount);
            if (updateSuccess == false)
            {
                result = OrderSubmitResult.PersistenceFailed;
                _logger.LogError($"Update `TradeHall` failed.");
                return false;
            }
            // 当可交易数量为 0 时，将交易信息状态更改为已完成交易
            if (restAmount == 0)
            {
                var failed = _hallRepos.UpdateState(data.Hall.TradeId, TradeHallState.Completed) == false;
                if (failed)
                {
                    result = OrderSubmitResult.PersistenceFailed;
                    _logger.LogError($"Update `TradeHall` state failed.");
                    return false;
                }
            }

            result = OrderSubmitResult.Success;
            return true;
        }

        private bool IsOrderStateNormal(long orderId, TradeOrderState matchState, out TradeOrder order, out ChangeOrderStateResult result)
        {
            order = _orderRepos.Find(orderId);
            if (order == null)
            {
                result = ChangeOrderStateResult.OrderNotExists;
                return false;
            }
            if (order.State != matchState)
            {
                result = ChangeOrderStateResult.OrderStateException;
                return false;
            }

            result = default(ChangeOrderStateResult);
            return true;
        }

        private bool AddOrderLog(long orderId, string log, int operatingUserId, string operatingUsername = "")
        {
            var model = new TradeOrderLog
            {
                CreateTime = DateTime.Now,
                OperateId = operatingUserId,
                OperateName = operatingUsername,
                OperateLog = log,
                OrderId = orderId,
            };
            var logId = _orderLogRepos.Insert(model);
            return logId > 0;
        }

        /// <summary>
        /// 确认收款操作
        /// </summary>
        /// <business>
        /// 业务逻辑：
        ///     1. 如果 orderId <= 0，则失败（避免无效查询）
        ///     2. 如果 operatingUserId <= 0，则失败（避免无效查询）
        ///     3. 如果订单不存在，则失败
        ///     4. 如果操作人不是卖家，则失败
        ///     5. 如果订单状态异常（不为待收款），则失败
        ///     6. 尝试更新订单状态，如果失败，则返回请求失败
        ///     7. 尝试添加订单操作日志，如果失败，则返回请求失败
        /// </business>
        /// <param name="orderId"></param>
        /// <param name="operatingUserId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [SmartSqlTransaction]
        public virtual bool Confirm(long orderId, int operatingUserId, out ChangeOrderStateResult result)
        {
            Check.EnsureMoreThanZero(orderId, nameof(orderId));
            Check.EnsureMoreThanZero(operatingUserId, nameof(operatingUserId));

            var canConfirm = IsOrderStateNormal(orderId, TradeOrderState.BuyerPaying, out var order, out result);
            if (canConfirm == false)
            {
                return false;
            }
            if (order.SellerUserId != operatingUserId)
            {
                result = ChangeOrderStateResult.Unautherized;
                return false;
            }

            var updateSuccess = _orderRepos.UpdateState(orderId, TradeOrderState.PlatformOperating);
            if (updateSuccess == false)
            {
                _logger.LogError($"Update order state failed.");
                result = ChangeOrderStateResult.PersistenceFailed;
                return false;
            }

            var addSuccess = AddOrderLog(orderId, "买家确认收款", operatingUserId);
            if (addSuccess == false)
            {
                _logger.LogError($"Add order log failed.");
                result = ChangeOrderStateResult.PersistenceFailed;
                return false;
            }

            result = ChangeOrderStateResult.Success;
            return true;
        }

        /// <summary>
        /// 取消订单操作
        /// </summary>
        /// <business>
        /// 业务逻辑：
        ///     1. 如果 orderId <= 0，则失败（避免无效查询）
        ///     2. 如果 operatingUserId <= 0，则失败（避免无效查询）
        ///     3. 如果订单不存在，则失败
        ///     4. 如果操作人不是卖家或者买家，则失败
        ///     5. 如果订单状态异常（不为待转币），则失败
        ///     6. 尝试更新订单状态，如果失败，则返回请求失败
        ///     7. 尝试添加订单操作日志，如果失败，则返回请求失败
        /// <param name="orderId"></param>
        /// <param name="operatingUserId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [SmartSqlTransaction]
        public virtual bool Cancel(long orderId, int operatingUserId, out ChangeOrderStateResult result)
        {
            Check.EnsureMoreThanZero(orderId, nameof(orderId));
            Check.EnsureMoreThanZero(operatingUserId, nameof(operatingUserId));

            var canCancel = IsOrderStateNormal(orderId, TradeOrderState.SellerOperating, out var order, out result);
            if (canCancel == false)
            {
                return false;
            }
            if (order.SellerUserId != operatingUserId && order.BuyerUserId != operatingUserId)
            {
                result = ChangeOrderStateResult.Unautherized;
                return false;
            }

            var updateSuccess = _orderRepos.UpdateState(orderId, TradeOrderState.Canceled);
            if (updateSuccess == false)
            {
                _logger.LogError($"Update order state failed.");
                result = ChangeOrderStateResult.PersistenceFailed;
                return false;
            }

            var log = operatingUserId == order.SellerUserId ? "卖家取消订单" : "买家取消订单";
            var addSuccess = AddOrderLog(orderId, log, operatingUserId);
            if (addSuccess == false)
            {
                _logger.LogError($"Add order log failed.");
                result = ChangeOrderStateResult.PersistenceFailed;
                return false;
            }

            result = ChangeOrderStateResult.Success;
            return true;
        }
    }
}
