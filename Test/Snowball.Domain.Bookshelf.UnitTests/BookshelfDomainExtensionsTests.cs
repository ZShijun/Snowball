using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Snowball.Core.Utils;
using Snowball.Domain.Bookshelf.Dtos.Options;
using Snowball.Domain.Bookshelf.Repositories;
using Snowball.Domain.Bookshelf.Services;
using Xunit;

namespace Snowball.Domain.Bookshelf.UnitTests
{
    public class BookshelfDomainExtensionsTests
    {
        [Fact]
        public void AddBookshelfDomain_ShouldBeBookService_WhenGetIBookService()
        {
            IServiceCollection services = CreateServiceCollection();
            services.AddBookshelfDomain();
            var bookService = services.BuildServiceProvider().GetRequiredService<IBookService>();
            Assert.IsAssignableFrom<BookService>(bookService);
        }

        [Fact]
        public void AddBookshelfDomain_ShouldBeWechatService_WhenGetIWechatService()
        {
            IServiceCollection services = CreateServiceCollection();
            services.AddBookshelfDomain();
            var wechatService = services.BuildServiceProvider().GetRequiredService<IWechatService>();
            Assert.IsAssignableFrom<WechatService>(wechatService);
        }

        private IServiceCollection CreateServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();
            return services.AddSingleton(Substitute.For<IBookRepository>())
                .AddSingleton(Substitute.For<IMapper>())
                .AddSingleton(Substitute.For<TimeProvider>())
                .AddSingleton(Substitute.For<IOptions<WechatOption>>())
                .AddSingleton(Substitute.For<ILogger<WechatService>>());
        }
    }
}
