using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.Admin.Models
{
    public class UserResponse
    {
        ///<summary>
        /// 主键
        ///</summary>
        public virtual int UserId { get; set; }
        ///<summary>
        /// 昵称
        ///</summary>
        public virtual string Nickname { get; set; }
        ///<summary>
        /// 头像地址
        ///</summary>
        public virtual string Avatar { get; set; }

        ///<summary>
        /// 手机号
        ///</summary>
        public virtual string Phone { get; set; }
        ///<summary>
        /// WalletAddress, varchar
        ///</summary>
        public virtual string WalletAddress { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        public virtual DateTime CreatedTime { get; set; }

    }
}
