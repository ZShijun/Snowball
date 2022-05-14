using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
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

        private IServiceCollection CreateServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();
            return services.AddSingleton(Substitute.For<IBookRepository>())
                .AddSingleton(Substitute.For<IMapper>());
        }
    }
}
