using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using NSubstitute;
using Snowball.Core.Data;
using System.Data;
using Xunit;

namespace Snowball.Core.UnitTests.Data
{
    public class BaseRepositoryTests
    {
        [Fact]
        public void CreateDbConnection_ShouldBeMySqlConnection_WhenGivenMySqlConnectionFactory()
        {
            var option = new ConnectionStringOption
            {
                Default = "server=127.0.0.1;database=test;uid=root;pwd=;"
            };

            IDbConnectionFactory connectionFactory = new MySqlConnectionFactory(option);
            MockRepository mockRepository = new MockRepository(connectionFactory);
            IDbConnection connection = mockRepository.CreateDbConnection();
            Assert.IsType<MySqlConnection>(connection);
        }
    }

    public class MockRepository : BaseRepository
    {
        public MockRepository(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }
    }
}
