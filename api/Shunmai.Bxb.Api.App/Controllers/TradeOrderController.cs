﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using Shunmai.Bxb.Api.App.Constansts;
using Shunmai.Bxb.Api.App.Models.Request;
using Shunmai.Bxb.Api.App.Utils;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Common.Constans;
using Shunmai.Bxb.Services.Enums;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shunmai.Bxb.Common.Extensions;

namespace Shunmai.Bxb.Api.App.Controllers
{
    [Route("orders")]
    public class TradeOrderController : AppBaseController
    {
        private readonly ILogger _logger;
        private readonly TradeOrderService _orderService;
        private readonly TradeHallService _hallService;

        public TradeOrderController(
            ILogger<TradeOrderController> logger
            , TradeOrderService orderService
            , TradeHallService tradeHallService
        )
        {
            _logger = logger;
            _orderService = orderService;
            _hallService = tradeHallService;
        }

        [HttpPost]
        public JsonResult Submit([FromBody]SubmitRequest request, [FromServices]UserService userService, [FromServices]SystemConfigService configService)
        {
            var hall = _hallService.GetById(request.TradeId);
            if (hall == null)
            {
                return Failed(Errors.TradeHallNotExists);
            }

            // 卖家信息
            var seller = userService.FindById(hall.ReleaseUserId);
            if (seller == null)
            {
                _logger.LogError($"Cannot find seller({hall.ReleaseUserId}) from db.");
                return Failed();
            }

            // 平台钱包地址
            var platWalletAddr = configService.GetPlatformWalletAddr()?.PlatWalletAddr;
            if (platWalletAddr.IsEmpty())
            {
                _logger.LogError($"Platform wallet address has not filled.");
                return Failed();
            }

            // 收取手续费的钱包地址
            var serviceFeeWalletAddr = configService.GetServiceFeeWalletAddr()?.PlatWalletAddr;
            if (serviceFeeWalletAddr.IsEmpty())
            {
                _logger.LogError($"Service fee wallet address has not filled.");
                return Failed();
            }

            // 手续费
            var tradeFeeConfig = configService.GetTradeFeeConfig();
            var serviceFee = (tradeFeeConfig == null)
                ? 0
                : (request.RequiredCount * tradeFeeConfig.SigleServiceFee) + tradeFeeConfig.SigleTradeFee;

            var submitData = new SubmitData
            {
                Buyer = CurrentUser,
                Hall = hall,
                PlatformWalletAddr = platWalletAddr,
                RequiredCount = request.RequiredCount,
                Seller = seller,
                ServiceFee = serviceFee,
                ServiceFeeReceiveWalletAddr = serviceFeeWalletAddr,
                TradeCode = request.TradeCode,
            };
            var success = _orderService.Submit(submitData, out OrderSubmitResult result);
            var errorInfo = ErrorInfoHelper.FromSubmitResult(result);
            return success ? Success() : Failed(errorInfo);
        }

        /// <summary>
        /// 确认收款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("{orderId:long}/confirm")]
        public JsonResult Confirm(long orderId)
        {
            var success = _orderService.Confirm(orderId, CurrentUser.UserId, out var result);
            var error = ErrorInfoHelper.FromConfirmResult(result);
            return success ? Success() : Failed(error);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("{orderId:long}/cancel")]
        public JsonResult Cancel(long orderId)
        {
            var success = _orderService.Cancel(orderId, CurrentUser.UserId, out var result);
            var error = ErrorInfoHelper.FromConfirmResult(result);
            return success ? Success() : Failed(error);
        }
    }
}
