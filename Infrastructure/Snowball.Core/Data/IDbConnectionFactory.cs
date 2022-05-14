using System.Data;

namespace Snowball.Core.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection();
    }
}
