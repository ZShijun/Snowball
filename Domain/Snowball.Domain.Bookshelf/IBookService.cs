using Snowball.Domain.Bookshelf.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Domain.Bookshelf
{
    public interface IBookService
    {
        Task<BookDto> GetAsync(int id);
    }
}
