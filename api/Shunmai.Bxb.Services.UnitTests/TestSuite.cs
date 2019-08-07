using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NLog.Extensions.Logging;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Services.Models.IET;
using System;
using System.Reflection;

namespace Shunmai.Bxb.Services.UnitTests
{
    public class TestSuite
    {
        public static IConfiguration Configuration => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        /// <summary>
        /// 注入SmartSql Respository
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterRepository(IServiceCollection services)
        {
            services
                .AddSmartSql()
                .AddRepositoryFromAssembly((options) =>
                {
                    options.AssemblyString = "Shunmai.Bxb.Repositories";
                });
        }

        public const string TEST_WALLET_ADDRESS = "jhq6NBPzBj4d9f5rsuh7mShzugP7cNEmKR";

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterService(IServiceCollection services)
        {
            services.AddSingleton<ICache>(sp => TestCache.GetInstance());
            services.AddSingleton(sp =>
            {
                var logger = Mock.Of<ILogger<IETService>>();
                var cache = sp.GetService<ICache>();
                return new IETService(logger, cache);
            });
        }

        private static IServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    var services = new ServiceCollection();
                    services
                        .AddOptions()
                        .AddLogging(builder =>
                        {
                            builder.ClearProviders();
                            builder.AddNLog();
                            builder.SetMinimumLevel(LogLevel.Debug);
                        });
                    
                    RegisterRepository(services);
                    RegisterService(services);

                    _serviceProvider = services.BuildServiceProvider();
                }
                return _serviceProvider;
            }
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        public static object InvokeMethod(object o, string method, params object[] args)
        {
            var type = o.GetType();
            try
            {
                return type.InvokeMember(method
                    , BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public
                    , null
                    , o
                    , args);
            }
            catch (Exception)
            {
                return default(string);
            }
        }
    }
}
