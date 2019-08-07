using Shunmai.Bxb.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.Admin.Models
{
    public class TradeOrderRequest
    {

        public long OrderId { get; set; }

        public TradeOrderState State { get; set; }
    }
}
