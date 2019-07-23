﻿using AspectCore.Extensions.DependencyInjection;
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
using Shunmai.Bxb.Common.Constants;
using Shunmai.Bxb.Common.Dto;
using Shunmai.Bxb.Common.Filters;
using Shunmai.Bxb.Common.Middleware;
using Shunmai.Bxb.Repositories.DIExtenssions;
using System;
using System.Threading;

namespace Shunmai.Bxb.Api.App
{
    public class Startup
    {
        const string CORS_NAME = "AllowAllOrigin";
        const string ALI_OSS_CONFIG_NAME = "AliOssConfig";
        const string JSON_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        const string NLOG_CONFIG_FILE_NAME = "NLog.config";

        private void AddServices(IServiceCollection services)
        {

        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ICache, AppRedisCache>()
                .AddHttpContextAccessor()
                .AddCors(
                    options => options.AddPolicy(
                    CORS_NAME,
                    builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials())
                )
                .Configure<AliOssServiceConfig>(Configuration.GetSection(ALI_OSS_CONFIG_NAME))
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
