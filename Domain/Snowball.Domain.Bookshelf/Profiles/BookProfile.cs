using AutoMapper;
using Snowball.Domain.Bookshelf.Dtos;
using Snowball.Domain.Bookshelf.Entities;

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
