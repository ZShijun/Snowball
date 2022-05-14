using Dapper;
using Snowball.Core.Data;
using Snowball.Domain.Wechat.Entities;
using Snowball.Domain.Wechat.Repositories;
using System.Data;
using System.Threading.Tasks;

namespace Snowball.Repositories.Wechat
{
    public class WechatRepository : BaseRepository, IWechatRepository
    {
        public WechatRepository(IDbConnectionFactory dbConnectionFactory)
               : base(dbConnectionFactory)
        {

        }

        public async Task<bool> ExistsSubscriptionAsync(string subscriber)
        {
            string sql = "SELECT COUNT(1) FROM Wechat_Subscription WHERE Subscriber=@Subscriber AND IsDel=0;";
            using IDbConnection dbConnection = CreateDbConnection();
            long count = await dbConnection.ExecuteScalarAsync<long>(sql, new { Subscriber = subscriber });
            return count > 0;
        }

        public async Task<bool> AddSubscriptionAsync(WechatSubscriptionEntity entity)
        {
            string sql = "INSERT INTO Wechat_Subscription(Provider,Subscriber) VALUE(@Provider,@Subscriber);";
            using IDbConnection dbConnection = CreateDbConnection();
            int count = await dbConnection.ExecuteAsync(sql, entity);
            return count > 0;
        }

        public async Task<bool> RemoveSubscriptionAsync(string subscriber)
        {
            string sql = "UPDATE Wechat_Subscription SET ModifyTime=CURRENT_TIMESTAMP,IsDel=1 WHERE Subscriber=@Subscriber AND IsDel=0;";
            using IDbConnection dbConnection = CreateDbConnection();
            int count = await dbConnection.ExecuteAsync(sql, new { Subscriber = subscriber });
            return count > 0;
        }

        public async Task<bool> AddSuggestionAsync(WechatSuggestionEntity entity)
        {
            string sql = "INSERT INTO Wechat_Suggestion(OpenId,Content) VALUE(@OpenId,@Content);";
            using IDbConnection dbConnection = CreateDbConnection();
            int count = await dbConnection.ExecuteAsync(sql, entity);
            return count > 0;
        }
    }
}
