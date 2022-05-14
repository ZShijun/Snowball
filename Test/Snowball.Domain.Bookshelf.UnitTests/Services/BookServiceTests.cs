using AutoMapper;
using NSubstitute;
using Snowball.Core;
using Snowball.Domain.Bookshelf.Entities;
using Snowball.Domain.Bookshelf.Profiles;
using Snowball.Domain.Bookshelf.Repositories;
using Snowball.Domain.Bookshelf.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Snowball.Domain.Bookshelf.UnitTests.Services
{
    public class BookServiceTests
    {
        [Theory]
        [InlineData(10001)]
        public async Task GetAsync_ShouldBeNotNull_WhenGivenBookIdExist(int bookId)
        {
            IBookService bookService = CreateBookService();
            var book = await bookService.GetAsync(bookId);
            Assert.NotNull(book);
        }

        [Theory]
        [InlineData(10000)]
        [InlineData(10002)]
        public async Task GetAsync_ShouldBeNull_WhenGivenBookIdNotExist(int bookId)
        {
            IBookService bookService = CreateBookService();
            var book = await bookService.GetAsync(bookId);
            Assert.Null(book);
        }

        [Fact]
        public async Task FuzzySearchByNameAsync_ShouldBeEmpty_WhenGivenBookNameKeyWordNotExist()
        {
            IBookService bookService = CreateBookService();
            var books = await bookService.FuzzySearchByNameAsync("刑法");
            Assert.Empty(books);
        }

        [Fact]
        public async Task FuzzySearchByNameAsync_ShouldThrowBizException_WhenBookCountIsMoreThen10()
        {
            IBookService bookService = CreateBookService();
            var ex = await Assert.ThrowsAsync<BizException>(() => bookService.FuzzySearchByNameAsync("财务自由"));
            Assert.Equal("大佬，多输入几个字吧，太难找了...", ex.Message);
        }

        [Fact]
        public async Task FuzzySearchByNameAsync_ShouldReturnNormally_WhenBookCountIsLessThen11()
        {
            IBookService bookService = CreateBookService();
            var books = await bookService.FuzzySearchByNameAsync("投资");
            Assert.Equal(2, books.Count());
        }

        private IBookService CreateBookService()
        {
            BookEntity book1 = new BookEntity
            {
                Id = 10001,
                Name = "聪明的投资者",
                Author = "本杰明·格雷厄姆"
            };
            BookEntity book2 = new BookEntity
            {
                Id = 10002,
                Name = "投资最重要的事",
                Author = "霍华德·马克斯"
            };
            IBookRepository bookRepository = Substitute.For<IBookRepository>();
            bookRepository.GetAsync(10001).Returns(book1);

            bookRepository.FuzzyCountByNameAsync("投资").Returns(2);
            bookRepository.FuzzyCountByNameAsync("财务自由").Returns(11);
            bookRepository.FuzzyCountByNameAsync("刑法").Returns(0);
            bookRepository.FuzzySearchByNameAsync("投资").Returns(new List<BookEntity> { book1,book2 });

            var configurationExpression = new MapperConfigurationExpression();
            configurationExpression.AddProfile<BookProfile>();
            var config = new MapperConfiguration(configurationExpression);
            var mapper = config.CreateMapper();
            return new BookService(bookRepository, mapper);
        }
    }
}
