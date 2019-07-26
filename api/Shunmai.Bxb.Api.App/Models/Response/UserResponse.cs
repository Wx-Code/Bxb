using System;

namespace Shunmai.Bxb.Api.App.Models.Response
{
    public class UserResponse
    {
        public int UserId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Realname { get; set; }
        ///<summary>
        /// 微信二维码图片
        ///</summary>
        public virtual string WxCodePhoto { get; set; }
        ///<summary>
        /// 微信开放平台标识（平台唯一）
        ///</summary>
        public virtual string WxUnionId { get; set; }
        ///<summary>
        /// 微信公众号用户标识（公众号内唯一）
        ///</summary>
        public virtual string WxOpenId { get; set; }
        ///<summary>
        /// 手机号
        ///</summary>
        public virtual string Phone { get; set; }
        ///<summary>
        /// WalletAddress, varchar
        ///</summary>
        public virtual string WalletAddress { get; set; }
        ///<summary>
        /// OutTotalAmount, decimal
        ///</summary>
        public virtual decimal OutTotalAmount { get; set; }
        ///<summary>
        /// InTotalAmount, decimal
        ///</summary>
        public virtual decimal InTotalAmount { get; set; }
        ///<summary>
        /// 创建时间
        ///</summary>
        public virtual DateTime CreatedTime { get; set; }
    }
}
