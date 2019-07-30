using Shunmai.Bxb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models
{
    public class SubmitData
    {
        public TradeHall Hall { get; set; }
        public User Buyer { get; set; }
        public User Seller { get; set; }
        public int RequiredCount { get; set; }
        public string TradeCode { get; set; }
        /// <summary>
        /// 收取手续费的钱包地址
        /// </summary>
        public string ServiceFeeReceiveWalletAddr { get; set; }
        /// <summary>
        /// 平台钱包地址
        /// </summary>
        public string PlatformWalletAddr { get; set; }
        /// <summary>
        /// 服务费比例
        /// </summary>
        public decimal ServiceFeeRate { get; set; }
        public decimal ServiceFee => ServiceFeeRate * RequiredCount;
    }
}
