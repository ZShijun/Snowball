using Microsoft.Extensions.DependencyInjection;
using Snowball.Domain.Wechat.Services;

namespace Snowball.Domain.Wechat
{
    public static class WechatDomainExtensions
    {
        public static IServiceCollection AddWechatDomain(this IServiceCollection services)
        {
            return services.AddSingleton<IWechatService, WechatService>();
        }
    }
}
