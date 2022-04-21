using AutoMapper;
using Snowball.Bookshelf.Domain.Entities;
using Snowball.Domain.Bookshelf.Dtos;

namespace Snowball.Domain.Bookshelf.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookEntity, BookDto>();

            CreateMap<BookDto, BookEntity>();
        }
    }
}
