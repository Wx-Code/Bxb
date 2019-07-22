using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using System;

namespace Shunmai.Bxb.Timer.Utils
{
    public static class SenparcExtenssions
    {
        public static IServiceCollection AddSenparc(this IServiceCollection services, IConfiguration configuration)
        {
            var dt1 = SystemTime.Now;

            //更多绑定操作参见：https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2
            var senparcSetting = new SenparcSetting();
            var senparcWeixinSetting = new SenparcWeixinSetting();

            configuration.GetSection("SenparcSetting").Bind(senparcSetting);
            configuration.GetSection("SenparcWeixinSetting").Bind(senparcWeixinSetting);

            services.AddMemoryCache();

            /*
            * CO2NET 是从 Senparc.Weixin 分离的底层公共基础模块，经过了长达 6 年的迭代优化，稳定可靠。
            * 关于 CO2NET 在所有项目中的通用设置可参考 CO2NET 的 Sample：
            * https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
            */

            services.AddSenparcGlobalServices(configuration);//Senparc.CO2NET 全局注册

            // 启动 CO2NET 全局注册，必须！
            IRegisterService register = RegisterService.Start(senparcSetting)
                                                        //关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
                                                        .UseSenparcGlobal();

            register.ChangeDefaultCacheNamespace("DefaultCO2NETCache");
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting);

            return services;
        }
    }
}
