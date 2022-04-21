using System.Data;

namespace Snowball.Core.Data
{
    public class DbConnectionFactory
    {
        public static IDbConnection Create(ConnectionStringOptions options)
        {
            IDbConnection connection = options.Provider switch
            {
                DbProvider.MySql => new MySql.Data.MySqlClient.MySqlConnection(options.ConnectionString),
                _ => new NullDbConnection(),
            };

            return connection;
        }
    }
}
