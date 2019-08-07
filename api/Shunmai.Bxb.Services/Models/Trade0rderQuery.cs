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

        public long OrderId { get; set; }

        //public decimal Amount { get; set; }

        //public decimal Price { get; set; }

        //public string Nickname { get; set; }

        //public DateTime CreateTime { get; set; }

        //public DateTime? ReceivedTime { get; set; }


        /// <summary>
        /// 状态
        /// </summary> 
        public TradeOrderState? Status { get; set; }

        public int[] adminStatu { get; set; }

        /// <summary>
        /// 交易码
        /// </summary>
        public string TradeCode { get; set; }

        public string SellerNickname { get; set; }

        public string BuyerNickname { get; set; }

        public string SellerWalletAddress { get; set; }

        public string BuyerWalletAddress { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 卖家手机号
        /// </summary>
        public string SellerPhone { get; set; }

        /// <summary>
        /// 买家手机号
        /// </summary>
        public string BuyerPhone { get; set; }
    }
}