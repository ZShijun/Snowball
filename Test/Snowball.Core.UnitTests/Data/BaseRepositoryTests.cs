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
            IOptions<ConnectionStringOption> options = Substitute.For<IOptions<ConnectionStringOption>>();
            options.Value.Returns(new ConnectionStringOption
            {
                Default = "server=127.0.0.1;database=test;uid=root;pwd=;"
            });

            IDbConnectionFactory connectionFactory = new MySqlConnectionFactory();
            MockRepository mockRepository = new MockRepository(connectionFactory, options);
            IDbConnection connection = mockRepository.CreateDbConnection();
            Assert.IsType<MySqlConnection>(connection);
        }
    }

    public class MockRepository : BaseRepository
    {
        public MockRepository(IDbConnectionFactory dbConnectionFactory, IOptions<ConnectionStringOption> options)
            : base(dbConnectionFactory, options)
        {
        }
    }
}
