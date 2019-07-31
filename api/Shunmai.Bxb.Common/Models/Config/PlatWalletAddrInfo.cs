using Shunmai.Bxb.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Common.Models.Config
{
    /// <summary>
    /// 平台钱包地址配置文件 返回字段
    /// </summary>
    public class PlatWalletAddrInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string PlatWalletAddrId { get; set; }

        /// <summary>
        /// 平台钱包地址地址名称
        /// </summary>
        public string PlatWalletAddrName { get; set; }

        /// <summary>
        /// 钱包地址
        /// </summary>
        public string PlatWalletAddr { get; set; }

        /// <summary>
        /// 状态  0  使用中   1 已停用
        /// </summary>
        public ConfigState State { get; set; }

        /// <summary>
        /// 用途  1、  转币    2、 手续费
        /// </summary>
        public PurposeType Purpost { get; set; }

        /// <summary>
        /// 请求所需的 Cookie 值
        /// </summary>
        public string Cookie { get; set; }


        /// <summary>
        /// IET 钱包 ID
        /// </summary>
        public string WalletId { get; set; }
        /// <summary>
        /// 交易密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
    }
}
