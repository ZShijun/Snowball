using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Core.Data
{
    public class ConnectionStringOptions
    {
        public string ConnectionString { get; set; }

        public DbProvider Provider { get; set; }
    }
}
