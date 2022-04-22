using Microsoft.Extensions.DependencyInjection;
using Snowball.Domain.Bookshelf.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Repositories.Bookshelf
{
    public static class BookshelfRepositoryExtensions
    {
        public static IServiceCollection AddBookshelfRepository(this IServiceCollection services)
        {
            return services.AddSingleton<IBookRepository, BookRepository>();
        }
    }
}
