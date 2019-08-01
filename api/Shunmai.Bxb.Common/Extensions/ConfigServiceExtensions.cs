using Shunmai.Bxb.Common.Constans;
using Shunmai.Bxb.Common.Enums;
using Shunmai.Bxb.Common.Models.Config;
using Shunmai.Bxb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shunmai.Bxb.Common.Extensions
{
    public static class ConfigServiceExtensions
    {
        private static PlatWalletAddrInfo GetWalletConfig(SystemConfigService service, PurposeType type)
        {
            var list = service.GetConfig<List<PlatWalletAddrInfo>>(SystemConfigNames.PLATFORM_WALLET_ADDRESS) ?? new List<PlatWalletAddrInfo>();
            return list.FirstOrDefault(c => c.Purpost == type && c.State == ConfigState.Normal);
        }

        private static CustomerServiceInfo GetCustomerServiceConfig(SystemConfigService service)
        {
            var obj = service.GetConfig<CustomerServiceInfo>(SystemConfigNames.CUSTOMER_SERVICE);
            return obj;
        }


        /// <summary>
        /// 获取平台钱包配置
        /// </summary>
        /// <param name="service"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static PlatWalletAddrInfo GetPlatformWalletAddr(this SystemConfigService service)
        {
            return GetWalletConfig(service, PurposeType.TurnCoin);
        }


        /// <summary>
        /// 获取平台收取手续费钱包配置
        /// </summary>
        /// <param name="service"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static PlatWalletAddrInfo GetServiceFeeWalletAddr(this SystemConfigService service)
        {
            return GetWalletConfig(service, PurposeType.CommissionCharge);
        }

        /// <summary>
        ///获取微信客服信息配置
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static CustomerServiceInfo GetServiceCustomer(this SystemConfigService service)
        {
            return GetCustomerServiceConfig(service);
        }

        /// <summary>
        /// 获取平台交易手续费配置
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static TradeFeeInfo GetTradeFeeConfig(this SystemConfigService service)
        {
            return service.GetConfig<TradeFeeInfo>(SystemConfigNames.TRADE_FEE);
        }
    }
}
