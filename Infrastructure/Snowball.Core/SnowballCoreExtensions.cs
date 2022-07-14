using Microsoft.Extensions.DependencyInjection;
using Snowball.Core.Cache;
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

        public static IServiceCollection AddCache(this IServiceCollection services, Action<CacheOption> options)
        {
            if (options == null)
            {
                return services;
            }

            var option = new CacheOption();
            options.Invoke(option);
            switch (option.CacheType)
            {
                case CacheType.Local:
                    break;
                case CacheType.Remote:
                    var remoteCache = new RemoteCache(option.ConnectionString);
                    services.AddSingleton<ICache>(remoteCache);
                    break;
                case CacheType.Hybird:
                    break;
                default:
                    break;
            }

            return services;
        }
    }
}
