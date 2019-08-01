using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Common.Models.Config
{
    /// <summary>
    /// 微信客服配置
    /// </summary>
    public class CustomerServiceInfo
    {
        /// <summary>
        /// 客服电话
        /// </summary>
        public string CustomerTel { get; set; }

        /// <summary>
        /// 微信客服号列表
        /// </summary>
        public List<WXCustomerCode> WXCustomerList { get; set; }


        /// <summary>
        /// 微信客服号信息
        /// </summary>
        public class WXCustomerCode
        {
            /// <summary>
            /// 微信客服号编号
            /// </summary>
            public int WXCustomerId { get; set; }

            /// <summary>
            /// 微信客服号
            /// </summary>
            public string WXCustomerNumber { get; set; }

            /// <summary>
            /// 是否使用  true: 使用中；   false : 已停用
            /// </summary>
            public bool IsChecked { get; set; }


        }

    }
}
