using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Quartz.Impl;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Cache;
using Shunmai.Bxb.Repositories.DIExtenssions;
using Shunmai.Bxb.Timer.Utils;
using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Timer
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        private static LogLevel DEFAULT_LOG_LEVEL = LogLevel.Debug;
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task Main()//string[] args
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceProvider = ConfigureServices();
            var factory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = factory.CreateLogger<Program>();

            var sf = new StdSchedulerFactory();
            var sched = await sf.GetScheduler();
            sched.JobFactory = new JobFactory(serviceProvider);
            logger.LogInformation("timer program started");
            await sched.Start();

            while (true)
            {
                Thread.Sleep(TimeSpan.FromHours(1));
            }
        }

        private static LogLevel GetDefaultLogLevel()
        {
            var section = Configuration.GetSection("Logging")?.GetSection("LogLevel");
            if (section == null)
            {
                return DEFAULT_LOG_LEVEL;
            }
            var @default = section["Default"];
            if (@default.IsEmpty())
            {
                return DEFAULT_LOG_LEVEL;
            }
            if (Enum.TryParse<LogLevel>(@default, out var level))
            {
                return level;
            }
            return DEFAULT_LOG_LEVEL;
        }

        private static IServiceProvider ConfigureServices()
        {            
            var services = new ServiceCollection();
            services.AddOptions()
                .AddSingleton<ICache, RedisCache>();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddNLog();
                builder.SetMinimumLevel(GetDefaultLogLevel());
            });
            services
                .AddSenparc(Configuration);


            services.AddSmartSqlRepositories();
            RegisterService(services);
            RegisterJob(services);

            return services.BuildAspectInjectorProvider();
        }

        private static void RegisterService(IServiceCollection services)
        {

        }

        private static void RegisterJob(IServiceCollection services)
        {

        }
    }
}
