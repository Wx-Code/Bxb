using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Common.Models.Config
{
    /// <summary>
    /// 手续费配置
    /// </summary>
    public class  TradeFeeInfo
    {
        /// <summary>
        /// 单笔转币手续费
        /// </summary>
        public decimal SigleTradeFee { get; set; }

        /// <summary>
        /// 单笔转币服务费 【备注：该字段代表百分比， 数据库存储为小数。】
        /// </summary>
        public decimal SigleServiceFee { get; set; }
    }
}
