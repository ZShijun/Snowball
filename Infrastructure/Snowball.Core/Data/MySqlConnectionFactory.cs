using System.Data;

namespace Snowball.Core.Data
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new MySql.Data.MySqlClient.MySqlConnection(connectionString);
        }
    }
}