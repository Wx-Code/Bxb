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
using Shunmai.Bxb.Common.Constants;
using Shunmai.Bxb.Common.Filters;
using Shunmai.Bxb.Common.Middleware;
using System;
using System.Threading;

namespace Shunmai.Bxb.Api.App
{
    public class Startup
    {
        const string CORS_NAME = "AllowAllOrigin";

        private void AddServices(IServiceCollection services)
        {

        }

        private void AddSmartSqlRepositories(IServiceCollection services)
        {
            services
                .AddSmartSql()
                .AddRepositoryFromAssembly((options) =>
                {
                    options.AssemblyString = Names.REPOSITORY_ASSEMBLY_NAME;
                });
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
                .AddMvc(options =>
                {
                    options.Filters.Add<UnhandledExceptionFilter>();
                    options.Filters.Add<LoginVerificationFilter>();
                })
                .AddJsonOptions(json =>
                {
                    json.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // SmartSql
            AddSmartSqlRepositories(services);
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
            env.ConfigureNLog("NLog.config");

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseCors(CORS_NAME);
            app.UseMvc();
        }
    }
}
