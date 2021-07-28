using Domain.Entities.Banks;
using Domain.Entities.Users;
using Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Domain.Services.Internal
{
    public class AdministratorService
    {
        // Fields
        private readonly IUnitOfWork UnitOfWork;

        private readonly IBankRepository BankRepository;
        private readonly IUserRepository UserRepository;

        // Constructors
        public AdministratorService(
            IUnitOfWork unitOfWork, 
            IBankRepository bankRepository, 
            IUserRepository userRepository
        )
        {
            UnitOfWork = unitOfWork;
            BankRepository = bankRepository;
            UserRepository = userRepository;
        }

        // Methods

        /// <exception cref="InvalidOperationException">
        ///     Bank already exists.
        /// </exception>
        public async Task CreateBankAsync(BankType bankType, string bankName, bool isEnabled = true)
        {
            var bank = new Bank(bankType, bankName, isEnabled);
            await UnitOfWork.StartTransactionAsync();
            bool bankExists = await BankRepository.ExistsAsync(bankType);
            if (bankExists)
            {
                throw new InvalidOperationException($"{nameof(Bank)} ({bankType}) already exists.");
            }
            await BankRepository.InsertAsync(bank);
            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        /// <exception cref="ArgumentException">
        ///     Bank was not found.
        /// </exception>
        public async Task UpdateBankNameAsync(BankType bankType, string bankName)
        {
            await UnitOfWork.StartTransactionAsync();
            Bank bank = await GetBankAsync(bankType);
            bank.Update(bankName);
            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        /// <exception cref="ArgumentException">
        ///     Bank was not found.
        /// </exception>
        public async Task DisableBankAsync(BankType bankType)
        {
            await UnitOfWork.StartTransactionAsync();
            Bank bank = await GetBankAsync(bankType);
            bank.Disable();
            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        /// <exception cref="ArgumentException">
        ///     Bank was not found.
        /// </exception>
        public async Task EnableBankAsync(BankType bankType)
        {
            await UnitOfWork.StartTransactionAsync();
            Bank bank = await GetBankAsync(bankType);
            bank.Enable();
            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        /// <exception cref="ArgumentException">
        ///     User was not found.
        /// </exception>
        public async Task LockUserWalletAsync(Guid userId, string reason)
        {
            await UnitOfWork.StartTransactionAsync();
            User user = await GetUserAsync(userId);
            user.LockWallet(reason);
            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        /// <exception cref="ArgumentException">
        ///     User was not found.
        /// </exception>
        public async Task UnlockUserWalletAsync(Guid userId, string reason)
        {
            await UnitOfWork.StartTransactionAsync();
            User user = await GetUserAsync(userId);
            user.UnlockWallet(reason);
            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        // Helper methods

        /// <exception cref="ArgumentException">
        ///     Bank was not found.
        /// </exception>
        private async Task<Bank> GetBankAsync(BankType bankType)
        {
            Bank bank = await BankRepository.GetByIdAsync(bankType);
            if (bank == null)
            {
                throw new ArgumentException($"{nameof(User)} (\"{bankType}\") was not found.", nameof(bankType));
            }
            return bank;
        }

        /// <exception cref="ArgumentException">
        ///     User was not found.
        /// </exception>
        private async Task<User> GetUserAsync(Guid userId)
        {
            User user = await UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"{nameof(User)} (\"{userId}\") was not found.", nameof(userId));
            }
            return user;
        }
    }
}
