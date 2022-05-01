using AutoMapper;
using Snowball.Core;
using Snowball.Domain.Bookshelf.Dtos;
using Snowball.Domain.Bookshelf.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snowball.Domain.Bookshelf.Services
{
    internal class BookService : IBookService
    {
        /// <summary>
        /// 模糊搜索条数限制
        /// </summary>
        private static readonly int _fuzzySearchLimit = 10;

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

        public async Task<IEnumerable<BookDto>> FuzzySearchByNameAsync(string bookName)
        {
            var count = await this._bookRepository.FuzzyCountByNameAsync(bookName);
            if (count == 0)
            {
                return Enumerable.Empty<BookDto>();
            }

            if (count > _fuzzySearchLimit)
            {
                throw new BizException("大佬，您输入的关键词太简短了，太难找了...");
            }

            var entities = await this._bookRepository.FuzzySearchByNameAsync(bookName);
            return this._mapper.Map<IEnumerable<BookDto>>(entities);
        }
    }
}
