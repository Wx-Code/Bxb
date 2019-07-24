using Shunmai.Bxb.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models
{
    /// <summary>
    ///  ConfigValue -->  平台钱包地址   【转币、 手续费】
    /// </summary>
    public class PlatWalletAddrConfigModel
    {
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
        /// TOKEN
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用途  1、  转币    2、 手续费
        /// </summary>
        public PurposeType Purpost { get; set; }




    }
}
