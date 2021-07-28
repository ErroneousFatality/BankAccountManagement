using Domain.Entities.Banks;
using System;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IBankRepository : IRepository<Bank, BankType>
    {
        Task<bool> ExistsAsync(BankType id);
    }
}
