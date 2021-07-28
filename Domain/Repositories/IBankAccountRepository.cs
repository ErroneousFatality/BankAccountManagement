using Domain.Entities.BankAccounts;
using Domain.Entities.Banks;
using System;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IBankAccountRepository : IRepository<BankAccount, Guid>
    {
        Task<bool> ExistsAsync(Guid userId, BankType bankId, string bankAccountNumber);
    }
}
