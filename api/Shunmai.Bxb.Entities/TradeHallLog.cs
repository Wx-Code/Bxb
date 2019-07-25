using System;

namespace Shunmai.Bxb.Entities
{
    ///<summary>
    /// 交易日志
    ///</summary>
    public class TradeHallLog
    {
        ///<summary>
        /// 主键
        ///</summary>
        public long LogId { get; set; }

        ///<summary>
        /// 交易大厅ID
        ///</summary>
        public int TradeHallId { get; set; }

        ///<summary>
        /// 操作人
        ///</summary>
        public int OperateId { get; set; }

        ///<summary>
        /// 操作人名称
        ///</summary>
        public string OperateName { get; set; }

        ///<summary>
        /// 操作内容
        ///</summary>
        public string OperateLog { get; set; }

        ///<summary>
        /// 操作时间
        ///</summary>
        public DateTime CreateTime { get; set; }
    }
}

