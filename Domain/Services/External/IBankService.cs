using System.Threading.Tasks;

namespace Domain.Services.External
{
    public interface IBankService
    {
        /// <exception cref="ArgumentException">Invalid pin.</exception>
        Task<decimal> CheckBalanceAsync(string uniqueMasterCitizenNumber, string pin);

        /// <exception cref="ArgumentException">Invalid pin.</exception>
        Task DepositAsync(string uniqueMasterCitizenNumber, string pin, decimal amount);

        /// <exception cref="ArgumentException">Invalid pin.</exception>
        /// <exception cref="InvalidOperationException">Insufficient funds.</exception>
        Task WithdrawAsync(string uniqueMasterCitizenNumber, string pin, decimal amount);
    }
}
