using Domain.Entities.Banks;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class EfBankRepository : EntityFrameworkRepository<Bank, BankType>, IBankRepository
    {
        // Constructors
        public EfBankRepository(EfContext context) : base(context)
        {

        }

        // Methods
        public async Task<bool> ExistsAsync(BankType bankId)
        {
            bool exists = await DbSet.AnyAsync(bank => bank.Type == bankId);
            return exists;
        }
    }
}
