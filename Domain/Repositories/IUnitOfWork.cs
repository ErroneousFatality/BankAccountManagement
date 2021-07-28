using System;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        /// <exception cref="InvalidOperationException">Transaction already started.</exception>
        Task StartTransactionAsync();

        Task SaveChangesAsync();

        /// <exception cref="InvalidOperationException">Transaction must be started before it can be commited.</exception>
        Task CommitTransactionAsync();

        /// <exception cref="InvalidOperationException">Transaction must be started before it can be commited.</exception>
        Task SaveChangesAndCommitTransactionAsync();

        /// <exception cref="InvalidOperationException">Transaction must be started before it can be aborted.</exception>
        Task AbortTransactionAsync();
    }
}
