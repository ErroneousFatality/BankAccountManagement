using Domain.Entities.BankAccounts;
using Domain.Entities.Banks;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class EfBankAccountRepository : EntityFrameworkRepository<BankAccount, Guid>, IBankAccountRepository
    {
        // Constructors
        public EfBankAccountRepository(EfContext context) : base(context)
        {

        }

        // Methods
        public async Task<bool> ExistsAsync(Guid userId, BankType bankId, string bankAccountNumber)
        {
            bool exists = await DbSet
                .AnyAsync(bankAccount
                    => bankAccount.UserId == userId
                    && bankAccount.BankId == bankId
                    && bankAccount.Number == bankAccountNumber
                );
            return exists;
        }
    }
}
