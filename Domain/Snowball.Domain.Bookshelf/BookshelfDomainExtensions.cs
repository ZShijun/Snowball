using Microsoft.Extensions.DependencyInjection;
using Snowball.Domain.Bookshelf.Services;

namespace Snowball.Domain.Bookshelf
{
    public static class BookshelfDomainExtensions
    {
        public static IServiceCollection AddBookshelfDomain(this IServiceCollection services)
        {
            return services.AddSingleton<IBookService, BookService>()
                .AddSingleton<IWechatService, WechatService>();
        }
    }
}
