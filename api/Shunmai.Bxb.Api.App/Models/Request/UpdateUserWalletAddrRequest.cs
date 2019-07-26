using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.App.Models.Request
{
    public class UpdateUserWalletAddrRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 钱包地址
        /// </summary>
        public string WalletAddress { get; set; }

    }
}
