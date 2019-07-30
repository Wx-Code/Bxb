using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using Shunmai.Bxb.Api.App.Constansts;
using Shunmai.Bxb.Api.App.Models.Request;
using Shunmai.Bxb.Api.App.Utils;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Constans;
using Shunmai.Bxb.Services.Enums;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var hall = _hallService.GetSingleTradeHallEntity(request.TradeHallId);
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
            var platWalletAddr = configService.GetConfig<string>(SystemConfigNames.PLATFORM_WALLET_ADDRESS);
            if (platWalletAddr.IsEmpty())
            {
                _logger.LogError($"Platform wallet address has not filled.");
                return Failed();
            }

            // 收取手续费的钱包地址
            var serviceFeeWalletAddr = configService.GetConfig<string>(SystemConfigNames.SERVICE_FEE_RECEIVE_WALLET_ADDRESS);
            if (serviceFeeWalletAddr.IsEmpty())
            {
                _logger.LogError($"Service fee wallet address has not filled.");
                return Failed();
            }

            // 手续费比例
            var serviceFeeRate = configService.GetConfig<decimal>(SystemConfigNames.TRADE_FEE);

            var submitData = new SubmitData
            {
                Buyer = CurrentUser,
                Hall = hall,
                PlatformWalletAddr = platWalletAddr,
                RequiredCount = request.Count,
                Seller = seller,
                ServiceFeeRate = serviceFeeRate,
                ServiceFeeReceiveWalletAddr = serviceFeeWalletAddr,
                TradeCode = request.Code,
            };
            var success = _orderService.Submit(submitData, out OrderSubmitResult result);
            var errorInfo = ErrorInfoHelper.FromSubmitResult(result);
            return Json(errorInfo);
        }
    }
}
