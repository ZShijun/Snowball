using Dapper;
using Microsoft.Extensions.Options;
using Snowball.Bookshelf.Domain.Entities;
using Snowball.Core.Data;
using Snowball.Domain.Bookshelf.Repositories;
using System.Data;
using System.Threading.Tasks;

namespace Snowball.Repositories.Bookshelf
{
    internal class BookRepository : BaseRepository, IBookRepository
    {
        public BookRepository(IDbConnectionFactory dbConnectionFactory, IOptions<ConnectionStringOption> options)
            : base(dbConnectionFactory, options)
        {

        }

        public Task<BookEntity> GetAsync(int id)
        {
            using (IDbConnection dbConnection = CreateDbConnection())
            {
                string sql = "select * from Book where Id=@Id;";
                return dbConnection.QueryFirstOrDefaultAsync<BookEntity>(sql, new { Id = id });
            }
        }
    }
}
