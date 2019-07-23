//*******************************
// Create By Ahoo Wang
// Date 2019-07-23 15:46
// Code Generate By SmartCode
// Code Generate Github : https://github.com/Ahoo-Wang/SmartCode
//*******************************

using System;
namespace Shunmai.Bxb.Entities
{

    ///<summary>
    /// Table, tradeorderlog
    ///</summary>
    public class TradeOrderLog
    {
        ///<summary>
        /// LogId, bigint
        ///</summary>
        public virtual long LogId { get; set; }
        ///<summary>
        /// OrderId, bigint
        ///</summary>
        public virtual long OrderId { get; set; }
        ///<summary>
        /// OperateId, int
        ///</summary>
        public virtual int OperateId { get; set; }
        ///<summary>
        /// OperateName, varchar
        ///</summary>
        public virtual string OperateName { get; set; }
        ///<summary>
        /// OperateLog, varchar
        ///</summary>
        public virtual string OperateLog { get; set; }
        ///<summary>
        /// CreateTime, datetime
        ///</summary>
        public virtual DateTime CreateTime { get; set; }
    }
}

