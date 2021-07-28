using Domain.Entities.BankTransactions;
using Domain.Repositories;
using System;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class EfBankTransactionRepository : EntityFrameworkRepository<BankTransaction, Guid>, IBankTransactionRepository
    {
        // Constructors
        public EfBankTransactionRepository(EfContext context) : base(context)
        {

        }
    }
}
