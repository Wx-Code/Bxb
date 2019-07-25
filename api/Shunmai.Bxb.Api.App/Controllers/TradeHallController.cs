using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.App.Constansts;
using Shunmai.Bxb.Api.App.Models;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Views;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models;

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
        [HttpPut("user/message")]
        public JsonResult PutMessage(TradeHall model)
        {
            TradeHall entity = _tradeHallService.GetSingleTradeHallEntity(model.TradeId);

            if (entity == null || entity.ReleaseUserId != CurrentUser.UserId)
                return Failed();

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
        public JsonResult GetMessage(Pager query)
        {
            (int num, List<TradeHallAppResponse> data) = _tradeHallService.PagedGetAppTradeHalls(query, null);

            foreach (TradeHallAppResponse item in data)
            {
                item.TradeCode = string.Empty;
                item.WxCodePhoto = string.Empty;
            }

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
        [HttpGet("message")]
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
        public JsonResult GetUserMessage(Pager query)
        {
            (int num, List<TradeHallAppResponse> data) = _tradeHallService.PagedGetAppTradeHalls(query, CurrentUser.UserId);

            foreach (TradeHallAppResponse item in data)
            {
                item.Nickname = string.Empty;
                item.Avatar = string.Empty;
                item.WxCodePhoto = string.Empty;
            }

            ListResponse<TradeHallAppResponse> result = new ListResponse<TradeHallAppResponse>
            {
                Total = num,
                List = data
            };
            return Success(result);
        }
    }
}