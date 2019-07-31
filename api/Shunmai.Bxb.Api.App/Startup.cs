using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Api.App.Cache;
using Shunmai.Bxb.Api.App.Filters;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Common.Filters;
using Shunmai.Bxb.Common.Middleware;
using Shunmai.Bxb.Repositories.DIExtenssions;
using Shunmai.Bxb.Services;
using System;
using System.Threading;
using Shunmai.Bxb.Utilities.Sms;
using Shunmai.Bxb.Services.Models.Wechat;
using Shunmai.Bxb.Services.Utils;

namespace Shunmai.Bxb.Api.App
{
    public class Startup
    {
        const string CORS_NAME = "AllowAllOrigin";
        const string ALI_OSS_CONFIG_NAME = "AliOssConfig";
        const string WECHAT_CONFIG_NAME = "Wechat";
        const string SMS_CONFIG_NAME = "SmsConfig";
        const string JSON_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        const string NLOG_CONFIG_FILE_NAME = "NLog.config";

        private void AddServices(IServiceCollection services)
        {
            services.AddSingleton<UserService>();
            services.AddSingleton(sp =>
            {
                var smsConfig = Configuration.GetSection(SMS_CONFIG_NAME).Get<SmsConfig>();
                return SmsProviderFactory.Create(smsConfig.SmsProvider);
            });
            services.AddSingleton<SmsService>();
            services.AddSingleton(sp =>
            {
                var config = Configuration.GetSection(WECHAT_CONFIG_NAME).Get<WechatConfig>();
                var factory = sp.GetRequiredService<ILoggerFactory>();
                return new WechatService(factory.CreateLogger<WechatService>(), config);
            });
            services.AddSingleton<TradeHallService>();
            services.AddSingleton<SystemConfigService>();
            services.AddSingleton<TradeOrderService>();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(NLOG_CONFIG_FILE_NAME).GetCurrentClassLogger();
            logger.Info("invoking ConfigureServices");
            services
                .AddSingleton<ICache, AppRedisCache>()
                .AddHttpContextAccessor()
                .AddCors(
                    options => options.AddPolicy(
                    CORS_NAME,
                    builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials())
                )
                .Configure<AliOssServiceConfig>(Configuration.GetSection(ALI_OSS_CONFIG_NAME))
                .Configure<SmsConfig>(Configuration.GetSection(SMS_CONFIG_NAME))
                .AddMvc(options =>
                {
                    options.Filters.Add<UnhandledExceptionFilter>();
                    options.Filters.Add<LoginVerificationFilter>();
                })
                .AddJsonOptions(json =>
                {
                    json.SerializerSettings.DateFormatString = JSON_TIME_FORMAT;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Senparc
            services.ConfigSenparc(Configuration);
            // SmartSql
            services.AddSmartSqlRepositories();
            AddServices(services);

            return services.BuildAspectInjectorProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app
            , IHostingEnvironment env
            , ILoggerFactory loggerFactory
        )
        {
            ThreadPool.SetMinThreads(384, 384);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddNLog();
            env.ConfigureNLog(NLOG_CONFIG_FILE_NAME);

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseCors(CORS_NAME);
            app.UseMvc();
        }
    }
}
