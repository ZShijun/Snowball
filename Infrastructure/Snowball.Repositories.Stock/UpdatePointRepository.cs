using Dapper;
using Snowball.Core.Data;
using Snowball.Domain.Stock.Entities;
using Snowball.Domain.Stock.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Repositories.Stock
{
    public class UpdatePointRepository : BaseRepository, IUpdatePointRepository
    {
        public UpdatePointRepository(IDbConnectionFactory dbConnectionFactory)
              : base(dbConnectionFactory)
        {

        }

        public Task<IEnumerable<UpdatePointEntity>> GetAllAsync()
        {
            string sql = @"SELECT * FROM Update_Point WHERE Enabled=1;";
            using IDbConnection dbConnection = CreateDbConnection();
            return dbConnection.QueryAsync<UpdatePointEntity>(sql);
        }

        public Task<UpdatePointEntity> GetAsync(string key)
        {
            string sql = @"SELECT * FROM Update_Point WHERE UpdateKey=@UpdateKey;";
            using IDbConnection dbConnection = CreateDbConnection();
            return dbConnection.QueryFirstOrDefaultAsync<UpdatePointEntity>(sql,
                new
                {
                    UpdateKey = key
                });
        }

        public async Task<bool> UpdateAsync(UpdatePointEntity entity)
        {
            string sql = @"UPDATE Update_Point SET UpdateTime=@UpdateTime,Enabled=@Enabled WHERE UpdateKey=@UpdateKey;";
            using IDbConnection dbConnection = CreateDbConnection();
            var count = await dbConnection.ExecuteAsync(sql, entity);
            return count > 0;
        }
    }
}
