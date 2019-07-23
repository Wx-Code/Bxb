//*******************************
// Create By Ahoo Wang
// Date 2019-07-23 15:46
// Code Generate By SmartCode
// Code Generate Github : https://github.com/Ahoo-Wang/SmartCode
//*******************************

using Shunmai.Bxb.Entities.Enums;
using System;
namespace SmartSql.Starter.Entity
{

    ///<summary>
    /// Table, tradehall
    ///</summary>
    public class TradeHall
    {
        ///<summary>
        /// TradeId, int
        ///</summary>
        public virtual int TradeId { get; set; }
        ///<summary>
        /// 交易类型
        ///</summary>
        public virtual TradeType TradeType { get; set; }
        ///<summary>
        /// ReleaseUserId, int
        ///</summary>
        public virtual int ReleaseUserId { get; set; }
        ///<summary>
        /// ReleaseName, varchar
        ///</summary>
        public virtual string ReleaseName { get; set; }
        ///<summary>
        /// 货币类型
        ///</summary>
        public virtual CurrencyType BType { get; set; }
        ///<summary>
        /// TotalAmount, decimal
        ///</summary>
        public virtual decimal TotalAmount { get; set; }
        ///<summary>
        /// Amount, decimal
        ///</summary>
        public virtual decimal Amount { get; set; }
        ///<summary>
        /// Price, decimal
        ///</summary>
        public virtual decimal Price { get; set; }
        ///<summary>
        /// ReleaseTime, datetime
        ///</summary>
        public virtual DateTime ReleaseTime { get; set; }
        ///<summary>
        /// 交易状态
        ///</summary>
        public virtual TradeHallState State { get; set; }
        ///<summary>
        /// 数据状态
        ///</summary>
        public virtual TradeHallShelfStatus Status { get; set; }
        ///<summary>
        /// TradeCode, varchar
        ///</summary>
        public virtual string TradeCode { get; set; }
    }
}

