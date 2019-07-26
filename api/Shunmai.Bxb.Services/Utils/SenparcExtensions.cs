using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Utils
{
    public static class SenparcExtensions
    {
        /// <summary>
        /// Senparc 依赖注入，参考：https://www.cnblogs.com/szw/p/9265828.html
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigSenparc(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSenparcGlobalServices(configuration)
                .AddSenparcWeixinServices(configuration);

            var settings = new SenparcSetting();
            var weixinSettings = new SenparcWeixinSetting();
            RegisterService.Start(settings)
                .UseSenparcGlobal()
                .UseSenparcWeixin(weixinSettings, settings);
            return services;
        }
    }
}
