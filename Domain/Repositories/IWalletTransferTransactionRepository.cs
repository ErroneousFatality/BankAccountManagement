using Domain.Entities.WalletTransferTransactions;
using System;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IWalletTransferTransactionRepository : IRepository<WalletTransferTransaction, Guid>
    {
        Task<WalletTransferTransaction> GetLastUserWalletTransferTransactionAsync(Guid id);
    }
}
