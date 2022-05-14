using MySql.Data.MySqlClient;
using System.Data;

namespace Snowball.Core.Data
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly ConnectionStringOption _option;

        public MySqlConnectionFactory(ConnectionStringOption option)
        {
            this._option = option;
            CheckOption();
        }

        private void CheckOption()
        {
           using IDbConnection dbConnection = CreateDbConnection();
        }

        public IDbConnection CreateDbConnection()
        {
            return new MySqlConnection(this._option.Default);
        }
    }
}