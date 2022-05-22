using Dapper;
using Snowball.Core.Data;
using Snowball.Domain.Stock.Entities;
using Snowball.Domain.Stock.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Snowball.Repositories.Stock
{
    public class IndexValuationRepository : BaseRepository, IIndexValuationRepository
    {
        public IndexValuationRepository(IDbConnectionFactory dbConnectionFactory)
               : base(dbConnectionFactory)
        {

        }

        public async Task<bool> InsertAsync(IndexValuationEntity entity)
        {
            string sql = @"INSERT INTO 
                           Index_Valuation(Code,Title,Description,Style,PE,PEPercentile,PB,PBPercentile,ROE,DividendYield,PreparationTime)
                           VALUE(@Code,@Title,@Description,@Style,@PE,@PEPercentile,@PB,@PBPercentile,@ROE,@DividendYield,@PreparationTime);";
            using IDbConnection dbConnection = CreateDbConnection();
            int count = await dbConnection.ExecuteAsync(sql, entity);
            return count > 0;
        }

        public Task<IEnumerable<IndexValuationEntity>> GetAllAsync()
        {
            string sql = @"SELECT * FROM Index_Valuation WHERE IsDel=0;";
            using IDbConnection dbConnection = CreateDbConnection();
            return dbConnection.QueryAsync<IndexValuationEntity>(sql);
        }
    }
}
