using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Snowball.Core.Data
{
    public abstract class BaseRepository
    {
        protected IDbConnectionFactory DbConnectionFactory { get; private set; }

        public BaseRepository(IDbConnectionFactory dbConnectionFactory)
        {
            this.DbConnectionFactory = dbConnectionFactory;
        }

        public virtual IDbConnection CreateDbConnection()
        {
            return this.DbConnectionFactory.CreateDbConnection();
        }
    }
}
