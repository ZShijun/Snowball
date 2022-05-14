using Microsoft.Extensions.DependencyInjection;
using Snowball.Core.Data;
using System;

namespace Snowball.Core
{
    public static class SnowballCoreExtensions
    {
        public static IServiceCollection AddMySql(this IServiceCollection services, Action<ConnectionStringOption> options)
        {
            if (options == null)
            {
                return services;
            }

            var option = new ConnectionStringOption();
            options.Invoke(option);
            if (string.IsNullOrWhiteSpace(option.Default))
            {
                return services;
            }

            var factory = new MySqlConnectionFactory(option);
            return services.AddSingleton<IDbConnectionFactory>(factory);
        }
    }
}
