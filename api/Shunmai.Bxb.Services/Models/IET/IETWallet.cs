using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class IETWallet
    {
        /// <summary>
        /// IET 钱包 ID
        /// </summary>
        public string WalletId { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPassword { get; set; }
        /// <summary>
        /// 交易密码
        /// </summary>
        public string TradePassword { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 钱包地址
        /// </summary>
        public string WalletAddress { get; set; }
    }
}
