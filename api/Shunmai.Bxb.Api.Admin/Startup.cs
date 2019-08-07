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
using Shunmai.Bxb.Api.Admin.Cache;
using Shunmai.Bxb.Api.Admin.Filters;
using Shunmai.Bxb.Common.Filters;
using Shunmai.Bxb.Common.Middleware;
using Shunmai.Bxb.Common.ModelBinder;
using Shunmai.Bxb.Repositories.DIExtenssions;
using Shunmai.Bxb.Services;
using System;

namespace Shunmai.Bxb.Api.Admin
{
    public class Startup
    {
        const string CORS_NAME = "AllowAllOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ICache, AdminRedisCache>()
                .AddMvc
                (options =>
                {
                    options.Filters.Add<UnhandledExceptionFilter>();
                    options.Filters.Add<LoginVerificationFilter>();
                    options.ModelBinderProviders.Insert(0, new NullableIntModelBinderProvider());
                }
                )
                .AddJsonOptions(json =>
                {
                    json.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSmartSqlRepositories();
            AddServices(services);

            services.AddHttpContextAccessor();
            services.AddCors(
                options => options.AddPolicy(
                CORS_NAME,
                builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials())
            );

            return services.BuildAspectInjectorProvider();
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddSingleton<AdminUserService>();
            services.AddSingleton<SystemConfigService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<TradeHallService>();
            services.AddSingleton<TradeOrderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app
            , IHostingEnvironment env
            , ILoggerFactory loggerFactory
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddNLog();
            env.ConfigureNLog("NLog.config");

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseCors(CORS_NAME);
            app.UseMvc();
        }
    }
}
