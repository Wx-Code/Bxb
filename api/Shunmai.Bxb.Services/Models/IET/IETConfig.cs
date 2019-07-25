using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class IETConfig
    {
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
        private decimal _serviceFee;
        /// <summary>
        /// 服务费率 [0,1)
        /// </summary>
        public decimal ServiceFeeRate
        {
            get { return _serviceFee; }
            set
            {
                if (_serviceFee < 0 || _serviceFee >= 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _serviceFee = value;
            }
        }
        public string ServiceFeeReceiveAddr { get; set; }
    }
}
