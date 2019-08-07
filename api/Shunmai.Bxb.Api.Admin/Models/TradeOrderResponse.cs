using Shunmai.Bxb.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.Admin.Models
{
    public class TradeOrderResponse
    {

        public string OrderId { get; set; }

        /// <summary>
        /// 卖家名称    
        /// </summary>
        public string SellerNickname { get; set; }

        public string SellerPhone { get; set; }

        public string SellerWalletAddress { get; set; }
        public CurrencyType Btype { get; set; }
    

        public string BtypeTxt { get; set; }

        public string TradeCode { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public string BuyerNickname { get; set; }

        public string BuyerPhone { get; set; }
        public DateTime? CreateTime { get; set; }
        public string BuyerWalletAddress { get; set; }

        public TradeOrderState? State { get; set; }

        public string StateTxt { get; set; }

        public string SurplusTime { get; set; }


    }
}
