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
    /// Table, systemconfig
    ///</summary>
    public class SystemConfig
    {
        ///<summary>
        /// ConfigName, varchar
        ///</summary>
        public virtual string ConfigName { get; set; }
        ///<summary>
        /// ConfigValue, varchar
        ///</summary>
        public virtual string ConfigValue { get; set; }
        ///<summary>
        /// Remark, varchar
        ///</summary>
        public virtual string Remark { get; set; }
        ///<summary>
        /// 记录创建时间
        ///</summary>
        public virtual DateTime CreateTime { get; set; }
        ///<summary>
        /// 记录创建人
        ///</summary>
        public virtual string CreateUser { get; set; }
    }
}

