
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.Admin.Constants;
using Shunmai.Bxb.Api.Admin.Models;
using Shunmai.Bxb.Api.App.Utils;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Utilities.Extenssions;
using Util.Helpers;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    [Route("admin/orders")]

    public class TradeOrderController : AdminBaseController
    {

        private readonly ILogger _logger;
        private readonly TradeOrderService _orderService;

        public TradeOrderController(ILogger<TradeOrderController> logger, TradeOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet("list")]
        public IActionResult GetSellerTradeOrders([FromQuery]Trade0rderQuery query)
        {
            if (query.Status == Entities.Enums.TradeOrderState.Completed)
            {
                int[] s = { (int)Entities.Enums.TradeOrderState.PlatformOperating, (int)Entities.Enums.TradeOrderState.Completed };
                query.adminStatu = s;
            }
            if (query.Status == Entities.Enums.TradeOrderState.Canceled)
            {
                int[] s = { (int)Entities.Enums.TradeOrderState.Canceled };
                query.adminStatu = s;
            }
            if (query.Status == Entities.Enums.TradeOrderState.BuyerPaying)
            {
                int[] s = { (int)Entities.Enums.TradeOrderState.BuyerPaying };
                query.adminStatu = s;
            }
            if (query.Status == Entities.Enums.TradeOrderState.SellerOperating)
            {
                int[] s = { (int)Entities.Enums.TradeOrderState.SellerOperating };
                query.adminStatu = s;
            }

            var (total, list) = _orderService.QueryPage(query);


            var startTime = DateTime.Now;
            var tradeorderList = new List<TradeOrderResponse>();
            foreach (var item in list.MapToList<TradeOrderResponse>())
            {
                ///待转币剩余时间
                if (item.State == Entities.Enums.TradeOrderState.SellerOperating)
                {
                    item.SurplusTime = Regex.Replace((Defaults.SellerOperating_EXPIRES - (startTime - item.CreateTime)).ToString(), @"\.\d+$", string.Empty);
                }
                else
                {
                    item.SurplusTime = "";
                }

                item.BtypeTxt = item.Btype.GetDescription();

                if (item.State == Entities.Enums.TradeOrderState.Completed)
                {
                    item.StateTxt = "已转币";
                }
                if (item.State == Entities.Enums.TradeOrderState.PlatformOperating)
                {
                    item.StateTxt = "待转币";
                }

                tradeorderList.Add(item);
            }

            var data = new ListResponse<TradeOrderResponse>
            {
                List = tradeorderList, //list.MapToList<TradeOrderResponse>(),
                Total = total
            };
            return Success(data);
        }

        [HttpPost("confirm")]
        public IActionResult Confirm([FromBody] TradeOrderRequest data)
        {
            var order = data.MapTo<TradeOrder>();
            var success = _orderService.ConfirmShouBi(data.OrderId, data.State, out var result);

            var error = ErrorInfoHelper.FromConfirmResult(result);
            return success
                ? Success()
                : Failed(error);
        }


        [HttpGet("orderloglist")]
        public IActionResult GetTradeOrderLogList([FromQuery]Trade0rderQuery query)
        {
            var list = _orderService.GetTradeOrderLogList(query.OrderId);
            var data = new Models.ListResponse<TradeOrderLog>
            {
                List = list.MapToList<TradeOrderLog>(),
            };
            return Success(data);
        }

          private bool EnsureConfigPrepared(PlatWalletAddrInfo platConfig, PlatWalletAddrInfo feeConfig)
        {
            if (platConfig == null || feeConfig == null)
            {
                return false;
            }
            if (platConfig.Phone.IsEmpty()
                || platConfig.LoginPassword.IsEmpty()
                || platConfig.TransactionPassword.IsEmpty()
                || platConfig.WalletId.IsEmpty()
                || platConfig.PlatWalletAddr.IsEmpty())
            {
                return false;
            }
            if (feeConfig.PlatWalletAddr.IsEmpty())
            {
                return false;
            }
            return true;
        }

        [HttpPut("{orderId:long}/pay")]
        public async Task<JsonResult> PayCoinToUser(long orderId, [FromServices]IETService ietService, [FromServices]SystemConfigService configService)
        {
            var platConfig = configService.GetPlatformWalletConfig();
            var feeConfig = configService.GetServiceFeeWalletConfig();
            if (EnsureConfigPrepared(platConfig, feeConfig) == false)
            {
                _logger.LogError($"Payment terminated because the IET wallet configs were not prepared.");
                return Failed("平台钱包配置或手续费钱包配置未准备好");
            }

            var success = _orderService.SetPayed(orderId, LogonUser.AdminUserId, LogonUser.Username, out var order);
            if (success == false)
            {
                return Failed("操作失败");
            }

            var message = await DoTransferring(platConfig, feeConfig, order, ietService);
            // 即使转账失败，也返回成功，因为数据库状态已被修改
            return Success(message);
        }

        private async Task<string> DoTransferring(PlatWalletAddrInfo platConfig, PlatWalletAddrInfo feeConfig, TradeOrder order, IETService ietService)
        {
            var message = "转账结果：";
            var wallet = new IETWallet
            {
                Phone = platConfig.Phone,
                LoginPassword = platConfig.LoginPassword,
                TradePassword = platConfig.TransactionPassword,
                WalletAddress = platConfig.PlatWalletAddr,
                WalletId = platConfig.WalletId,
            };

            const string feeFailedMsg = "手续费转币失败，请联系相关人员进行处理。";
            try
            {
                var feePayed = await ietService.PayAsync(wallet, feeConfig.PlatWalletAddr, order.ServiceAmount, $"订单({order.OrderId})手续费");
                message += feePayed ? "手续费转币成功。" : feeFailedMsg;
                if (feePayed == false)
                {
                    _logger.LogError($"Transform coins from platform wallet to service fee wallet failed. OrderId: {order.OrderId}, Fee: {order.ServiceAmount}");
                }
            }
            catch (Exception ex)
            {
                message += feeFailedMsg;
                _logger.Exception($"An exception occurred on transferring coins to service fee wallet for order '{order.OrderId}.'", ex);
            }

            const string userFailedMsg = "向用户转币失败，请联系相关人员进行处理。";
            try
            {
                var userPayed = await ietService.PayAsync(wallet, order.BuyerWalletAddress, order.Amount, $"订单({order.OrderId})转账");
                message += userPayed ? "向用户转币成功。" : userFailedMsg;
                if (userPayed == false)
                {
                    _logger.LogError($"Transform coins from platform wallet to user wallet failed. OrderId: {order.OrderId}, Amount: {order.Amount}");
                }
            }
            catch (Exception ex)
            {
                message += userFailedMsg;
                _logger.Exception($"An exception occurred on transferring coins to user wallet for order '{order.OrderId}.'", ex);
            }

            return message;
        }

    }
}
=======
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Common.Extensions;
using Shunmai.Bxb.Common.Models.Config;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models.IET;
using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    [Route("orders")]
    public class TradeOrderController : AdminBaseController
    {
        private readonly ILogger _logger;
        private readonly TradeOrderService _orderService;

        public TradeOrderController(ILogger<TradeOrderController> logger, TradeOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

      
    }
}
>>>>>>> 5905971a741b7205364de6432abd24dc7a379e6b
