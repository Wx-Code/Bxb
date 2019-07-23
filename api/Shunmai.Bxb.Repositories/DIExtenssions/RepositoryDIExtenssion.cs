using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shunmai.Bxb.Repositories.DIExtenssions
{
    public static class RepositoryDIExtenssion
    {
        public static IServiceCollection AddSmartSqlRepositories(this IServiceCollection services)
        {
            services
                .AddSmartSql()
                .AddRepositoryFromAssembly((options) =>
                {
                    options.AssemblyString = Assembly.GetExecutingAssembly().GetName().Name;
                });
            return services;
        }
    }
}
