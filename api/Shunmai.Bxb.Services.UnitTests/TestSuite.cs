using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
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

        public const string TEST_WALLET_ADDRESS = "jn2P895ePPzQXSwaXn7y7hUuT9YSJ5fb1w";

        /// <summary>
        /// 注入Service
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterService(IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var config = new IETConfig
                {
                    Cookie = "WEBID=db0d02b8-b81e-4c53-ab80-958f833e263e",
                    Password = "Jp2d\\/9Wb6YNGCxnJAWInpA==",
                    Phone = "15041113056",
                    ServiceFeeRate = 0.05m,
                    ServiceFeeReceiveAddr = TEST_WALLET_ADDRESS,
                    WalletId = "6da2540dadbe46d5b8eb6ad7d6c6944b"
                };
                return new IETService(config);
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
