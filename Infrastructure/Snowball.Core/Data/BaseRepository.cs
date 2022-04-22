using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Snowball.Core.Data
{
    public abstract class BaseRepository
    {
        protected ConnectionStringOption ConnectionStringOption { get; private set; }

        protected IDbConnectionFactory DbConnectionFactory { get; private set; }

        public BaseRepository(IDbConnectionFactory dbConnectionFactory, IOptions<ConnectionStringOption> options)
        {
            this.DbConnectionFactory = dbConnectionFactory;
            this.ConnectionStringOption = options.Value;
        }

        public virtual IDbConnection CreateDbConnection()
        {
            return this.DbConnectionFactory.CreateDbConnection(ConnectionStringOption.Default);
        }
    }
}
