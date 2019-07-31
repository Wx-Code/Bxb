﻿using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Attributes;
using Shunmai.Bxb.Services.Enums;
using Shunmai.Bxb.Services.Models;
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

        /// <summary>
        /// 尝试提交订单，如果出现如下情形之一，则返回 false 表示订单提交失败
        ///     1. 如果交易信息不存在
        ///     2. 如果交易信息的 `Status` 不处于上架状态
        ///     3. 如果交易信息的 `State` 不处于正常进行中
        ///     4. 如果输入的交易码错误
        ///     5. 如果输入的购买数量大于剩余数量
        ///     6. 尝试创建交易订单，如果创建失败
        ///     7. 尝试创建交易日志，如果创建失败
        /// </summary>
        /// <param name="hallId"></param>
        /// <param name="count"></param>
        /// <param name="tradeCode"></param>
        /// <param name="buyerId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [SmartSqlTransaction]
        public bool Submit(SubmitData data, out OrderSubmitResult result)
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

            var order = CreateOrder(data);
            var orderId = _orderRepos.Insert(order);
            if (orderId <= 0)
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
                OrderId = orderId,
            };
            var logId = _orderLogRepos.Insert(orderLog);
            if (logId <= 0)
            {
                result = OrderSubmitResult.PersistenceFailed;
                _logger.LogError($"Insert into `TradeOrderLog` failed.");
                return false;
            }

            result = OrderSubmitResult.Success;
            return true;
        }
    }
}