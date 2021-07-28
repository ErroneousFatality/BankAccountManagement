using Domain.Entities.FeeTransactions;
using Domain.Repositories;
using System;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class EfFeeTransactionRepository : EntityFrameworkRepository<FeeTransaction, Guid>, IFeeTransactionRepository
    {
        // Constructors
        public EfFeeTransactionRepository(EfContext context) : base(context)
        {

        }
    }
}
