using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    [Route("orders")]
    public class TradeOrderController : AdminBaseController
    {
        public TradeOrderController()
        {

        }

        [HttpPut("{orderId:long}/pay")]
        public JsonResult PayCoinToUser(long orderId)
        {
            throw new NotImplementedException();
        }
    }
}
