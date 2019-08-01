using Shunmai.Bxb.Entities;
using System;

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

        private decimal _serviceFee;
        public decimal ServiceFee
        {
            get { return _serviceFee; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _serviceFee = value;
            }
        }
    }
}
