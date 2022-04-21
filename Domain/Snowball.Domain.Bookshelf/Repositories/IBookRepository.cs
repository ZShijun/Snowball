using Snowball.Bookshelf.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Domain.Bookshelf.Repositories
{
    public interface IBookRepository
    {
        Task<BookEntity> GetAsync(int id);
    }
}
