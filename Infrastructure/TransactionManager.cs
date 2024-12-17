using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Infrastructure
{
    public class TransactionManager : IDisposable
    {
        private readonly DbContext _context;
        private IDbContextTransaction? _transaction;

        public TransactionManager(DbContext context)
        {
            _context = context;
        }

        // Bắt đầu Transaction
        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        // Commit Transaction
        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        // Rollback Transaction
        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        // Dispose
        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}
