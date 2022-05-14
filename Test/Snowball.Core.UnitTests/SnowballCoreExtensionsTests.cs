using Microsoft.Extensions.DependencyInjection;
using Snowball.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Snowball.Core.UnitTests
{
    public class SnowballCoreExtensionsTests
    {
        [Fact]
        public void AddMySql_ShouldBeNull_WhenGivenNullAndGetIDbConnectionFactory()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMySql(null);

            var dbConnectionFactory = services.BuildServiceProvider().GetService<IDbConnectionFactory>();
            Assert.Null(dbConnectionFactory);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddMySql_ShouldBeNull_WhenGivenNullOrEmptyConnectionStringAndGetIDbConnectionFactory(string connectionString)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMySql(options =>
            {
                options.Default = connectionString;
            });

            var dbConnectionFactory = services.BuildServiceProvider().GetService<IDbConnectionFactory>();
            Assert.Null(dbConnectionFactory);
        }

        [Fact]
        public void AddMySql_ShouldThrow_WhenGivenInvalidConnectionString()
        {
            string connectionString = "abc";
            IServiceCollection services = new ServiceCollection();
            Assert.Throws<ArgumentException>(()=>services.AddMySql(options =>
            {
                options.Default = connectionString;
            }));
        }

        [Fact]
        public void AddMySql_ShouldBeMySqlConnectionFactory_WhenGivenValidConnectionStringAndGetIDbConnectionFactory()
        {
            string connectionString = "server=127.0.0.1;database=test;uid=root;pwd=;";
            IServiceCollection services = new ServiceCollection();
            services.AddMySql(options =>
            {
                options.Default = connectionString;
            });

            var dbConnectionFactory = services.BuildServiceProvider().GetService<IDbConnectionFactory>();
            Assert.IsAssignableFrom<MySqlConnectionFactory>(dbConnectionFactory);
        }
    }
}
