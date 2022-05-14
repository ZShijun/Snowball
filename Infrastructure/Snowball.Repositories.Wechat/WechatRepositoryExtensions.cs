using Microsoft.Extensions.DependencyInjection;
using Snowball.Domain.Wechat.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Repositories.Wechat
{
    public static class WechatRepositoryExtensions
    {
        public static IServiceCollection AddWechatRepository(this IServiceCollection services)
        {
            return services.AddSingleton<IWechatRepository, WechatRepository>();
        }
    }
}
