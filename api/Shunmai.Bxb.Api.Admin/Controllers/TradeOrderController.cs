using Microsoft.AspNetCore.Mvc;
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
