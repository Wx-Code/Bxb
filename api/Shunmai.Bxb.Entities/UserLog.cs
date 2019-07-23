//*******************************
// Create By Ahoo Wang
// Date 2019-07-23 15:46
// Code Generate By SmartCode
// Code Generate Github : https://github.com/Ahoo-Wang/SmartCode
//*******************************

using Shunmai.Bxb.Entities.Enums;
using System;

namespace Shunmai.Bxb.Entities
{
    ///<summary>
    /// Table, userlog
    ///</summary>
    public class UserLog
    {
        ///<summary>
        /// UserLogId, bigint
        ///</summary>
        public virtual long UserLogId { get; set; }
        ///<summary>
        /// 用户 ID，关联用户表主键
        ///</summary>
        public virtual int UserId { get; set; }
        ///<summary>
        /// 日志类型
        ///</summary>
        public virtual UserLogType LogType { get; set; }
        ///<summary>
        /// 日志内容
        ///</summary>
        public virtual string LogContent { get; set; }
        ///<summary>
        /// 前端显示的日志内容
        ///</summary>
        public virtual string LogContentFront { get; set; }
        ///<summary>
        /// 操作人
        ///</summary>
        public virtual string Operator { get; set; }
        ///<summary>
        /// 创建时间
        ///</summary>
        public virtual DateTime CreatedTime { get; set; }
        ///<summary>
        /// 删除标识
        ///</summary>
        public virtual bool Deleted { get; set; }
    }
}

