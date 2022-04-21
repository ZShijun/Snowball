using Snowball.Domain.Bookshelf.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Application
{
    public interface IBookAppService
    {
        Task<BookDto> GetAsync(int id);
    }
}
