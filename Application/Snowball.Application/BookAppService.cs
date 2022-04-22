using Snowball.Domain.Bookshelf;
using Snowball.Domain.Bookshelf.Dtos;
using System.Threading.Tasks;

namespace Snowball.Application
{
    internal class BookAppService : IBookAppService
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
