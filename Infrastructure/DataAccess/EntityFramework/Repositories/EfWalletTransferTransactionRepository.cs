using Domain.Entities.WalletTransferTransactions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class EfWalletTransferTransactionRepository : EntityFrameworkRepository<WalletTransferTransaction, Guid>, IWalletTransferTransactionRepository
    {
        // Constructors
        public EfWalletTransferTransactionRepository(EfContext context) : base(context)
        {

        }

        // Methods
        public async Task<WalletTransferTransaction> GetLastUserWalletTransferTransactionAsync(Guid userId)
        {
            WalletTransferTransaction walletTransferTransaction = await DbSet
                .Where((WalletTransferTransaction walletTransferTransaction) => walletTransferTransaction.UserId == userId)
                .OrderByDescending((WalletTransferTransaction walletTransferTransaction) => walletTransferTransaction.DateTime)
                .FirstOrDefaultAsync();
            return walletTransferTransaction;
        }
    }
}
