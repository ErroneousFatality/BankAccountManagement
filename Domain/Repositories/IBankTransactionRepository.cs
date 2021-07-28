using Domain.Entities.BankTransactions;
using System;

namespace Domain.Repositories
{
    public interface IBankTransactionRepository : IRepository<BankTransaction, Guid>
    {
    }
}
