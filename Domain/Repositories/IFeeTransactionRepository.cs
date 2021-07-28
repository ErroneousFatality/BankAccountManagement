using Domain.Entities.FeeTransactions;
using System;

namespace Domain.Repositories
{
    public interface IFeeTransactionRepository : IRepository<FeeTransaction, Guid>
    {
    }
}
