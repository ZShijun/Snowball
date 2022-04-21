using Dapper;
using Snowball.Bookshelf.Domain.Entities;
using Snowball.Core.Data;
using Snowball.Domain.Bookshelf.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Repositories.Bookshelf
{
    public class BookRepository : BaseRepository, IBookRepository
    {
        public BookRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public Task<BookEntity> GetAsync(int id)
        {
            string sql = "select * from Book where Id=@Id;";
            return this.UnitOfWork.Connection.QueryFirstOrDefaultAsync<BookEntity>(sql, new { Id = id }, this.UnitOfWork.Transaction);
        }
    }
}
