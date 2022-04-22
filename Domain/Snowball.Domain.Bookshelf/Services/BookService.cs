using AutoMapper;
using Snowball.Domain.Bookshelf.Dtos;
using Snowball.Domain.Bookshelf.Repositories;
using System.Threading.Tasks;

namespace Snowball.Domain.Bookshelf.Services
{
    internal class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository
            , IMapper mapper)
        {
            this._bookRepository = bookRepository;
            this._mapper = mapper;
        }
        public async Task<BookDto> GetAsync(int id)
        {
            var entity = await this._bookRepository.GetAsync(id);
            return this._mapper.Map<BookDto>(entity);
        }
    }
}
