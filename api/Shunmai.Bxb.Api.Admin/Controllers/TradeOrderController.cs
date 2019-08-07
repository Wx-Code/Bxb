using System;
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

        private readonly ILogger<TradeOrderController> _logger;
        private readonly TradeOrderService _tradeOrderService;


        public TradeOrderController(ILogger<TradeOrderController> logger, TradeOrderService tradeOrderService)
        {
            _logger = logger;
            _tradeOrderService = tradeOrderService;
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

            var (total, list) = _tradeOrderService.QueryPage(query);


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
            var success = _tradeOrderService.ConfirmShouBi(data.OrderId, data.State, out var result);

            var error = ErrorInfoHelper.FromConfirmResult(result);
            return success
                ? Success()
                : Failed(error);
        }


        [HttpGet("orderloglist")]
        public IActionResult GetTradeOrderLogList([FromQuery]Trade0rderQuery query)
        {
            var list = _tradeOrderService.GetTradeOrderLogList(query.OrderId);
            var data = new Models.ListResponse<TradeOrderLog>
            {
                List = list.MapToList<TradeOrderLog>(),
            };
            return Success(data);
        }

    }
}