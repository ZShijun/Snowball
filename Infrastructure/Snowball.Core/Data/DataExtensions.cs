using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Snowball.Core.Data
{
    public static class DataExtensions
    {
        public static IServiceCollection ConfigureMySql<TOption>(this IServiceCollection services, IConfiguration configuration)
            where TOption : ConnectionStringOption
        {
            return services.Configure<TOption>(configuration.GetSection("ConnectionStrings"))
                 .AddSingleton<IDbConnectionFactory, MySqlConnectionFactory>();
        }
    }
}
