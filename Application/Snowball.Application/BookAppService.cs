using Snowball.Domain.Bookshelf;
using Snowball.Domain.Bookshelf.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Application
{
    public class BookAppService : IBookAppService
    {
        private readonly IBookService _bookService;

        public BookAppService(IBookService bookService)
        {
            this._bookService = bookService;
        }

        public Task<BookDto> GetAsync(int id)
        {
            return this._bookService.GetAsync(id);
        }
    }
}
