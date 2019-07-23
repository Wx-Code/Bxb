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
    /// Table, smslog
    ///</summary>
    public class SmsLog
    {
        ///<summary>
        /// LogId, bigint
        ///</summary>
        public virtual long LogId { get; set; }
        ///<summary>
        /// 发送的手机号
        ///</summary>
        public virtual string Phone { get; set; }
        ///<summary>
        /// 发送短信的类型：（0：验证码类型；1：通知A；）
        ///</summary>
        public virtual SmsType Type { get; set; }
        ///<summary>
        /// 短信内容
        ///</summary>
        public virtual string Content { get; set; }
        ///<summary>
        /// 签名
        ///</summary>
        public virtual string Sign { get; set; }
        ///<summary>
        /// 发送状态（0：失败；1：成功）
        ///</summary>
        public virtual SmsState State { get; set; }
        ///<summary>
        /// 第三方给运营商的时间
        ///</summary>
        public virtual DateTime SendTime { get; set; }
        ///<summary>
        /// 发送短信的应用
        ///</summary>
        public virtual ApplicationType RequestPlat { get; set; }
        ///<summary>
        /// 短信服务商（0：当前；1：阿里）
        ///</summary>
        public virtual SmsProvider Provider { get; set; }
    }
}

