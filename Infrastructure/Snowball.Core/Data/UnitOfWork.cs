using Microsoft.Extensions.Options;
using System.Data;

namespace Snowball.Core.Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection = null;
        private IDbTransaction _transaction = null;

        public IDbTransaction Transaction { get { return _transaction; } }
        public IDbConnection Connection { get { return _connection; } }

        public UnitOfWork(IOptions<ConnectionStringOptions> options)
        {
            _connection = DbConnectionFactory.Create(options.Value);
            _connection.Open();
        }

        public void Begin()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (_transaction == null)
                {
                    return;
                }

                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }
        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            if (_connection != null)
            {
                _connection.Dispose();
            }
            _connection = null;
            _transaction = null;
        }
    }
}
