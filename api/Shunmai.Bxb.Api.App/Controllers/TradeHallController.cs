using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.App.Constansts;
using Shunmai.Bxb.Api.App.Models;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Common.Constants;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Entities.Views;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Utilities.Extenssions;

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
        [HttpPost("user/message")]
        public JsonResult PostMessage([FromBody]TradeHall model)
        {
            if (!Enum.IsDefined(typeof(CurrencyType), model.BType) || model.Price <= 0m || model.Price >= Defaults.TRADE_MAX_AMOUNT || model.TotalAmount <= 0m || model.TotalAmount >= Defaults.TRADE_MAX_AMOUNT)
                return Failed(Errors.ParamsOutOfRange);

            if (string.IsNullOrWhiteSpace(CurrentUser.Phone))
                return Failed(Errors.UserNotRegister);

            if (string.IsNullOrWhiteSpace(CurrentUser.WalletAddress))
                return Failed(Errors.UserWalletAddressNotExists);

            model.ReleaseUserId = CurrentUser.UserId;
            model.ReleaseName = CurrentUser.Nickname;

            (int code, string message) = _tradeHallService.InsertTradeHallEntity(model);

            if (code == 201) return Success(model.TradeCode);

            _logger.LogError(message);
            return Failed("发布交易信息失败");
        }

        /// <summary>
        /// 编辑交易信息
        /// </summary>
        [HttpPut("user/message")]
        public JsonResult PutMessage([FromBody]TradeHall model)
        {
            if (!Enum.IsDefined(typeof(CurrencyType), model.BType) || model.Price <= 0m || model.Price >= Defaults.TRADE_MAX_AMOUNT || model.TotalAmount <= 0m || model.TotalAmount >= Defaults.TRADE_MAX_AMOUNT)
                return Failed(Errors.ParamsOutOfRange);

            TradeHall entity = _tradeHallService.GetSingleTradeHallEntity(model.TradeId);

            if (entity == null || entity.ReleaseUserId != CurrentUser.UserId)
                return Failed();

            model.ReleaseName = CurrentUser.Nickname;

            (int code, string message) = _tradeHallService.UpdateTradeHallEntity(model);

            if(code == 200) return Success();
            _logger.LogError(message);

            return Failed("编辑交易信息失败");
        }

        /// <summary>
        /// 分页获取交易大厅中的所有交易信息
        /// </summary>
        [HttpGet("message")]
        [SkipLoginVerification]
        public JsonResult GetMessage([FromQuery]TradeHallQuery query)
        {
            if (query.BType < 0 || query.BType > 4) return Failed("系统不存在该类型的币种");

            if (query.BType == 0) query.BType = null;

            (int num, List<TradeHallAppResponse> data) = _tradeHallService.PagedGetAppTradeHalls(query);

            ListResponse<TradeHallAppResponse> result = new ListResponse<TradeHallAppResponse>
            {
                Total = num,
                List = data
            };
            return Success(result);
        }

        /// <summary>
        /// 获取单条交易记录
        /// </summary>
        [HttpGet("message/{id}")]
        [SkipLoginVerification]
        public JsonResult GetMessage(int id)
        {
            if (id <= 0) return Failed();

            TradeHallAppResponse result = _tradeHallService.GetAppTradeHallDetail(id);

            if (result == null) return Failed();
            return Success(result);
        }

        /// <summary>
        /// 获取我发布的信息
        /// </summary>
        [HttpGet("user/message")]
        public JsonResult GetUserMessage([FromQuery]TradeHallQuery query)
        {
            query.UserId = CurrentUser.UserId;

            (int count, List<TradeHallAppResponse> data) = _tradeHallService.PageGetAdminTradeHalls(query);

            foreach (TradeHallAppResponse item in data)
            {
                item.StatusText = item.Status.GetDescription();
            }

            ListResponse<TradeHallAppResponse> result = new ListResponse<TradeHallAppResponse>
            {
                Total = count % query.Size == 0 ? count / query.Size : count / query.Size + 1,
                List = data
            };
            return Success(result);
        }

        /// <summary>
        /// 下架我发布的消息
        /// </summary>
        [HttpPost("user/message/putOff")]
        public JsonResult PutMessageStatus([FromBody]TradeHall model)
        {
            if (model.TradeId <= 0) return Failed();

            TradeHall entity = _tradeHallService.GetSingleTradeHallEntity(model.TradeId);

            if (entity == null || entity.ReleaseUserId != CurrentUser.UserId)
                return Failed();

            (int code, string message) = _tradeHallService.UpdateTradeHallStatus(model.TradeId, TradeHallShelfStatus.Off, CurrentUser.UserId, CurrentUser.Nickname);

            if (code == 200) return Success();
            _logger.LogError(message);

            return Failed("下架失败");
        }
    }
}