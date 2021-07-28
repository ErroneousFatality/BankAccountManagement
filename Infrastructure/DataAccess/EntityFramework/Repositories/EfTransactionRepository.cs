using Domain.Entities.Transactions;
using Domain.Entities.WalletTransferTransactions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class EfTransactionRepository : EntityFrameworkRepository<Transaction, Guid>, ITransactionRepository
    {
        // Constructors
        public EfTransactionRepository(EfContext context) : base(context)
        {

        }

        // Methods

        public async Task<ICollection<Transaction>> GetManyByUserIdAsync(Guid userId)
        {
            IQueryable<Transaction> userTransactionsQuery = DbSet
                .Where((Transaction transaction)
                    => transaction.UserId == userId 
                    || (transaction is WalletTransferTransaction && ((WalletTransferTransaction)transaction).DestinationUserId == userId)
                );
            IList<Transaction> userTransactions = await userTransactionsQuery.ToListAsync();
            return userTransactions;
        }

        public async Task<decimal> GetUserMonthlyDepositSumAsync(Guid userId)
        {
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            IQueryable<Transaction> userMonthlyDepositTransactionsQuery = DbSet
                .Where((Transaction transaction)
                    => (transaction.PaymentDirection == PaymentDirectionType.Deposit && transaction.UserId == userId)
                    || (transaction is WalletTransferTransaction && ((WalletTransferTransaction)transaction).DestinationUserId == userId)
                );
            decimal monthlyDepositSum = await userMonthlyDepositTransactionsQuery
                .SumAsync((Transaction transaction) => transaction.Amount);
            return monthlyDepositSum;
        }

        public async Task<decimal> GetUserMonthlyWithdrawTotalAsync(Guid userId)
        {
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            IQueryable<Transaction> userMonthlyWithdrawTransactionsQuery = DbSet
                .Where((Transaction transaction)
                    => (transaction.PaymentDirection == PaymentDirectionType.Withdraw && transaction.UserId == userId)
                );
            decimal monthlyDepositSum = await userMonthlyWithdrawTransactionsQuery
                .SumAsync((Transaction transaction) => transaction.Amount);
            return monthlyDepositSum;
        }
    }
}
