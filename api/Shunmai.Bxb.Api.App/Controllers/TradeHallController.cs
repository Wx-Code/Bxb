using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.App.Constansts;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Services;

namespace Shunmai.Bxb.Api.App.Controllers
{
    public class TradeHallController : AppBaseController
    {
        private readonly ILogger<TradeHallController> _logger;
        private readonly TradeHallService _tradeHallService;

        public TradeHallController(ILogger<TradeHallController> logger, TradeHallService tradeHallService)
        {
            _logger = logger;
            _tradeHallService = tradeHallService;
        }

        /// <summary>
        /// 发布交易信息
        /// </summary>
        [HttpPost("message")]
        public JsonResult PostMessage(TradeHall model)
        {
            if (string.IsNullOrWhiteSpace(CurrentUser.Phone))
                return Failed(Errors.UserNotRegister);

            if (string.IsNullOrWhiteSpace(CurrentUser.WalletAddress))
                return Failed(Errors.UserWalletAddressNotExists);

            model.ReleaseUserId = CurrentUser.UserId;
            model.ReleaseName = CurrentUser.Nickname;

            (int code, string message) = _tradeHallService.InsertTradeHallEntity(model);

            if (code == 201) return Success();

            _logger.LogError(message);
            return Failed("发布交易信息失败");
        }

        /// <summary>
        /// 编辑交易信息
        /// </summary>
        [HttpPut("message")]
        public JsonResult PutMessage(TradeHall model)
        {
            //todo：zhu 校验信息的发布人和当前人是否一样

            (int code, string message) = _tradeHallService.UpdateTradeHallEntity(model);

            if(code == 200) return Success();
            _logger.LogError(message);

            return Failed("编辑交易信息失败");
        }


    }
}