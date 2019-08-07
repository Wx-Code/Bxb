using Shunmai.Bxb.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Entities.Views
{
    public class TradeOrderResponse
    {

        ///<summary>
        /// OrderId, int
        ///</summary>
        public virtual long OrderId { get; set; }

        public virtual string SellerNickname { get; set; }

        public virtual string BuyerNickname { get; set; }
        ///<summary>
        /// TradeId, int
        ///</summary>
        public virtual int TradeId { get; set; }
        ///<summary>
        /// 0：卖出；
        ///             1：求购；
        ///</summary>
        public virtual TradeType TradeType { get; set; }
        ///<summary>
        /// SellerUserId, int
        ///</summary>
        public virtual int SellerUserId { get; set; }
        ///<summary>
        /// SellerWalletAddress, varchar
        ///</summary>
        public virtual string SellerWalletAddress { get; set; }
        ///<summary>
        /// SellerPhone, varchar
        ///</summary>
        public virtual string SellerPhone { get; set; }
        ///<summary>
        /// BuyerUserId, int
        ///</summary>
        public virtual int BuyerUserId { get; set; }
        ///<summary>
        /// BuyerWalletAddress, varchar
        ///</summary>
        public virtual string BuyerWalletAddress { get; set; }
        ///<summary>
        /// BuyerPhone, varchar
        ///</summary>
        public virtual string BuyerPhone { get; set; }
        ///<summary>
        /// Btype, smallint
        ///</summary>
        public virtual CurrencyType Btype { get; set; }
        ///<summary>
        /// Amount, decimal
        ///</summary>
        public virtual decimal Amount { get; set; }
        ///<summary>
        /// Price, decimal
        ///</summary>
        public virtual decimal Price { get; set; }
        ///<summary>
        /// TotalAmount, decimal
        ///</summary>
        public virtual decimal TotalAmount { get; set; }
        ///<summary>
        /// 订单状态
        ///</summary>
        public virtual TradeOrderState State { get; set; }
        ///<summary>
        /// TradeCode, varchar
        ///</summary>
        public virtual string TradeCode { get; set; }
        ///<summary>
        /// ServiceAmount, decimal
        ///</summary>
        public virtual decimal ServiceAmount { get; set; }
        ///<summary>
        /// PlatWalletAddress, varchar
        ///</summary>
        public virtual string PlatWalletAddress { get; set; }
        ///<summary>
        /// PlatServiceWalletAddress, varchar
        ///</summary>
        public virtual string PlatServiceWalletAddress { get; set; }
        ///<summary>
        /// CreateTime, datetime
        ///</summary>
        public virtual DateTime CreateTime { get; set; }
        ///<summary>
        /// ReceivedTime, datetime
        ///</summary>
        public virtual DateTime? ReceivedTime { get; set; }
        ///<summary>
        /// PayTime, datetime
        ///</summary>
        public virtual DateTime? PayTime { get; set; }
        ///<summary>
        /// CompleteTime, datetime
        ///</summary>
        public virtual DateTime? CompleteTime { get; set; }
    }
}
