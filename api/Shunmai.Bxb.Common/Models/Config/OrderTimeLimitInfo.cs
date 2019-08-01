using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Common.Models.Config
{
    /// <summary>
    /// 订单时间配置 
    /// </summary>
    public class OrderTimeLimitInfo
    {
        /// <summary>
        /// 卖家转币有效时间（小时） ， 数据库存储 单位： 毫秒
        /// </summary>
        public int SellerTrunCoinTime { get; set; }

        /// <summary>
        /// 卖家确认收款有效时间 (小时) ， 数据库存储 单位： 毫秒
        /// </summary>
        public int SellerReceiveMoneyTime { get; set; }


    }
}
