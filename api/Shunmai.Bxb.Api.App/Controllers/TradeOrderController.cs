using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.App.Constansts;
using Shunmai.Bxb.Api.App.Models.Request;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Enums;
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

        public TradeOrderController(ILogger<TradeOrderController> logger, TradeOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost]
        public JsonResult Submit([FromBody]SubmitRequest request)
        {
            if (CurrentUser.WalletAddress.IsEmpty())
            {
                return Failed(Errors.UserWalletAddressNotExists);
            }

            var success = _orderService.Submit(request.TradeHallId, request.Count, request.Code, out OrderSubmitResult result);

        }
    }
}
