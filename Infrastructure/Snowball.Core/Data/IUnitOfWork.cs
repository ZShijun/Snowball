using System;
using System.Data;

namespace Snowball.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction Transaction { get; }
        IDbConnection Connection { get; }
        void Begin();

        void Commit();
    }
}
