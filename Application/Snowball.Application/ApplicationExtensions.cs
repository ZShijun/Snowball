using Microsoft.Extensions.DependencyInjection;

namespace Snowball.Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddSingleton<IWechatAppService, WechatAppService>()
                .AddSingleton<IStockAppService, StockAppService>();
        }
    }
}
