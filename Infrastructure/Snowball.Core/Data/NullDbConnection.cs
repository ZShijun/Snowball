using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Snowball.Core.Data
{
    public class NullDbConnection : IDbConnection
    {
        public string ConnectionString { get; set; }

        public int ConnectionTimeout { get; }

        public string Database { get; }

        public ConnectionState State { get; }

        public IDbTransaction BeginTransaction()
        {
            return null;
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return null;
        }

        public void ChangeDatabase(string databaseName)
        {
        }

        public void Close()
        {
        }

        public IDbCommand CreateCommand()
        {
            return null;
        }

        public void Dispose()
        {
        }

        public void Open()
        {

        }
    }
}
