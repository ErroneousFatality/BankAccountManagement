using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        // Fields
        private DbContext Context;
        private IDbContextTransaction Transaction;

        // Constructors
        public EntityFrameworkUnitOfWork(DbContext dbContext)
        {
            Context = dbContext;
        }

        // Methods

        /// <exception cref="InvalidOperationException">Transaction already started.</exception>
        public async Task StartTransactionAsync()
        {
            if (Transaction != null)
            {
                throw new InvalidOperationException("Transaction already started.");
            }
            Transaction = await Context.Database.BeginTransactionAsync();
        }


        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        /// <exception cref="InvalidOperationException">Transaction must be started before it can be commited.</exception>
        public async Task CommitTransactionAsync()
        {
            if (Transaction == null)
            {
                throw new InvalidOperationException("Transaction must be started before it can be commited.");
            }
            await Transaction.CommitAsync();
            await DisposeTransactionAsync();
        }

        public async Task SaveChangesAndCommitTransactionAsync()
        {
            await SaveChangesAsync();
            await CommitTransactionAsync();
        }

        /// <exception cref="InvalidOperationException">Transaction must be started before it can be aborted.</exception>
        public async Task AbortTransactionAsync()
        {
            if (Transaction == null)
            {
                throw new InvalidOperationException("Transaction must be started before it can be commited.");
            }
            await Transaction.RollbackAsync();
            await DisposeTransactionAsync();
        }

        // Helper methods
        private async Task DisposeTransactionAsync()
        {
            await Transaction.DisposeAsync();
            Transaction = null;
        }

        #region IDisposable implementation

        private bool isDisposed = false; // To detect redundant calls

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            // dispose code goes below
            if (Transaction != null)
            {
                await AbortTransactionAsync();
            }
            // dispose code goes above

            Dispose(isDisposing: false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposed)
            {
                return;
            }
            if (isDisposing)
            {
                // dispose code goes below
                if (Transaction != null)
                {
                    AbortTransactionAsync().Wait();
                }
                // dispose code goes above
            }
            isDisposed = true;
        }

        #endregion IDisposable implementation
    }
}
