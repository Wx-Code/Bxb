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
            return list.FirstOrDefault(c => c.Purpost == type);
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
    }
}
