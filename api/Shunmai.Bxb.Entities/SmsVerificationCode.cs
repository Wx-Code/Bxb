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
    /// Table, smsverificationcode
    ///</summary>
    public class SmsVerificationCode
    {
        ///<summary>
        /// VcId, int
        ///</summary>
        public virtual int VcId { get; set; }
        ///<summary>
        /// Phone, varchar
        ///</summary>
        public virtual string Phone { get; set; }
        ///<summary>
        /// VerificationCode, varchar
        ///</summary>
        public virtual string VerificationCode { get; set; }
        ///<summary>
        /// State, smallint
        ///</summary>
        public virtual SmsCodeState State { get; set; }
        ///<summary>
        /// CreateTime, datetime
        ///</summary>
        public virtual DateTime CreateTime { get; set; }
    }
}

