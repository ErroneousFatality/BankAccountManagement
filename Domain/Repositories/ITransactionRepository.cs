using Domain.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction, Guid>
    {
        Task<decimal> GetUserMonthlyDepositSumAsync(Guid id);
        Task<decimal> GetUserMonthlyWithdrawTotalAsync(Guid id);
        Task<ICollection<Transaction>> GetManyByUserIdAsync(Guid userId);
    }
}
