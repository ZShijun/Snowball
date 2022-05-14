using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Snowball.Core.Utils;
using Snowball.Domain.Wechat.Dtos;
using Snowball.Domain.Wechat.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Snowball.Domain.Wechat.UnitTests
{
    public class WechatDomainExtensionsTests
    {
        [Fact]
        public void AddWechatDomain_ShouldBeWechatService_WhenGetIWechatService()
        {
            IServiceCollection services = CreateServiceCollection();
            services.AddWechatDomain();
            var wechatService = services.BuildServiceProvider().GetRequiredService<IWechatService>();
            Assert.IsAssignableFrom<WechatService>(wechatService);
        }

        private IServiceCollection CreateServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();
            return services.AddSingleton(Substitute.For<TimeProvider>())
            .AddSingleton(Substitute.For<IOptions<WechatOption>>())
            .AddSingleton(Substitute.For<ILogger<WechatService>>());
        }
    }
}
