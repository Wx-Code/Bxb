using Shunmai.Bxb.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models
{
    public class Trade0rderQuery : Pager
    {
        /// <summary>
        /// 发布人ID
        /// </summary>
        public int? UserId { get; set; }

        //public int TradeId { get; set; }

        //public long OrderId { get; set; }

        //public decimal Amount { get; set; }

        //public decimal Price { get; set; }

        //public string Nickname { get; set; }

        //public DateTime CreateTime { get; set; }

        //public DateTime? ReceivedTime { get; set; }


        /// <summary>
        /// 状态
        /// </summary> 
        public TradeOrderState? Status { get; set; }
    }
}