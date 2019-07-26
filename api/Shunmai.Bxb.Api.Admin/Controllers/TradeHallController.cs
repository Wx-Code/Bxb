using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.Admin.Models;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Entities.Views;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Utilities.Extenssions;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    public class TradeHallController : AdminBaseController
    {
        private readonly ILogger<TradeHallController> _logger;
        private readonly TradeHallService _tradeHallService;

        public TradeHallController(ILogger<TradeHallController> logger, TradeHallService tradeHallService)
        {
            _logger = logger;
            _tradeHallService = tradeHallService;
        }

        /// <summary>
        /// 获取交易信息列表
        /// </summary>
        [HttpGet("getTradeHallList")]
        public JsonResult GetTradeHallList([FromQuery] TradeHallQuery query)
        {
            (int count, List<TradeHallAppResponse> data) = _tradeHallService.PageGetAdminTradeHalls(query);

            foreach (TradeHallAppResponse item in data)
            {
                item.StatusText = item.Status.GetDescription();
            }

            ListResponse<TradeHallAppResponse> result = new ListResponse<TradeHallAppResponse>
            {
                Total = count,
                List = data
            };
            return Success(result);
        }

        /// <summary>
        /// 下架
        /// </summary>
        [HttpPost("putOff")]
        public JsonResult PutMessageStatus([FromBody]TradeHall model)
        {
            if (model.TradeId <= 0) return Failed();

            (int code, string message) = _tradeHallService.UpdateTradeHallStatus(model.TradeId, TradeHallShelfStatus.Off);

            if (code == 200) return Success();
            _logger.LogError(message);

            return Failed("下架失败");
        }
    }
}