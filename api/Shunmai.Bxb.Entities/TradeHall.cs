using Shunmai.Bxb.Entities.Enums;
using System;

namespace Shunmai.Bxb.Entities
{
    ///<summary>
    ///交易大厅
    ///</summary>
    public class TradeHall
    {
        ///<summary>
        /// 主键
        ///</summary>
        public int TradeId { get; set; }

        ///<summary>
        /// 交易类型
        ///</summary>
        public TradeType TradeType { get; set; }

        ///<summary>
        /// 发布者ID
        ///</summary>
        public int ReleaseUserId { get; set; }

        ///<summary>
        /// 发布者昵称
        ///</summary>
        public string ReleaseName { get; set; }

        ///<summary>
        /// 货币类型
        ///</summary>
        public CurrencyType BType { get; set; }

        ///<summary>
        /// 总数量
        ///</summary>
        public decimal TotalAmount { get; set; }

        ///<summary>
        /// 可交易数量
        ///</summary>
        public decimal Amount { get; set; }

        ///<summary>
        /// 单价
        ///</summary>
        public decimal Price { get; set; }

        ///<summary>
        /// 发布时间
        ///</summary>
        public DateTime ReleaseTime { get; set; }

        ///<summary>
        /// 交易状态
        ///</summary>
        public TradeHallState State { get; set; }

        ///<summary>
        /// 数据状态
        ///</summary>
        public TradeHallShelfStatus Status { get; set; }

        ///<summary>
        /// 交易码
        ///</summary>
        public string TradeCode { get; set; }
    }
}

