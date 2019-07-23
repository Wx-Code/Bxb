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
    /// Table, tradehalllog
    ///</summary>
    public class TradeHallLog
    {
        ///<summary>
        /// LogId, bigint
        ///</summary>
        public virtual long LogId { get; set; }
        ///<summary>
        /// TradeHallId, int
        ///</summary>
        public virtual int TradeHallId { get; set; }
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

