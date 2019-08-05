using Shunmai.Bxb.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.App.Models.Response
{
    public class PaltWalletAddressResponse
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
        /// 用途  1、  转币    2、 手续费
        /// </summary>
        public PurposeType Purpost { get; set; }
    }
}
