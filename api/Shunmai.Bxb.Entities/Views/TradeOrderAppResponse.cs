using Shunmai.Bxb.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Entities.Views
{
    public class TradeOrderAppResponse
    {

        /// <summary>
        /// 交易ID
        /// </summary>
        public int TradeId { get; set; }

        public long OrderId { get; set; }

        public decimal Amount { get; set; }

        public decimal Price { get; set; }

        public string Nickname { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ReceivedTime { get; set; }


        /// <summary>
        /// 状态
        /// </summary> 
        public TradeOrderState? State { get; set; }

        public string SurplusTime { get; set; }

        public string TradeCode { get; set; }

        public string Phone { get; set; }

        public CurrencyType Btype { get; set; }

        public string BtypeTxt { get; set; }

        public DateTime? CompleteTime { get; set; }

        public string PriceTxt { get; set; }

        public string AmountTxt { get; set; }


    }
}
