using Dapper;
using Snowball.Core.Data;
using Snowball.Domain.Bookshelf.Entities;
using Snowball.Domain.Bookshelf.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Snowball.Repositories.Bookshelf
{
    internal class BookRepository : BaseRepository, IBookRepository
    {
        public BookRepository(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {

        }

        public Task<BookEntity> GetAsync(int id)
        {
            using IDbConnection dbConnection = CreateDbConnection();
            string sql = "SELECT Id,`Name`,DownloadUrl,ExtractionCode FROM Book WHERE Id=@Id AND IsDel=0;";
            return dbConnection.QueryFirstOrDefaultAsync<BookEntity>(sql, new { Id = id });
        }

        public async Task<long> FuzzyCountByNameAsync(string bookName)
        {
            if (string.IsNullOrWhiteSpace(bookName))
            {
                return 0;
            }

            using IDbConnection dbConnection = CreateDbConnection();
            string sql = "SELECT COUNT(*) FROM Book WHERE `Name` LIKE CONCAT('%',@BookName,'%') AND IsDel=0";
            return await dbConnection.QueryFirstAsync<long>(sql, new { BookName = bookName.Trim() });
        }

        public async Task<IEnumerable<BookEntity>> FuzzySearchByNameAsync(string bookName)
        {
            if (string.IsNullOrWhiteSpace(bookName))
            {
                return Enumerable.Empty<BookEntity>();
            }

            using IDbConnection dbConnection = CreateDbConnection();
            string sql = "SELECT Id,`Name`,OriginalName,Author,Translator,DoubanScore FROM Book WHERE `Name` LIKE CONCAT('%',@BookName,'%') AND IsDel=0";
            return await dbConnection.QueryAsync<BookEntity>(sql, new { BookName = bookName.Trim() });
        }
    }
}
