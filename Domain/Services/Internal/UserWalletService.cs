using Domain.Entities.BankAccounts;
using Domain.Entities.Banks;
using Domain.Entities.BankTransactions;
using Domain.Entities.FeeTransactions;
using Domain.Entities.Transactions;
using Domain.Entities.Users;
using Domain.Entities.WalletTransferTransactions;
using Domain.Repositories;
using Domain.Services.External;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services.Internal
{
    public class UserWalletService
    {
        // Fields
        private readonly IUnitOfWork UnitOfWork;

        private readonly IBankRepository BankRepository;
        private readonly IBankAccountRepository BankAccountRepository;
        private readonly IBankTransactionRepository BankTransactionRepository;
        private readonly ITransactionRepository TransactionRepository;
        private readonly IUserRepository UserRepository;
        private readonly IWalletTransferTransactionRepository WalletTransferTransactionRepository;

        private readonly BankServiceFactory BankServiceFactory;

        public readonly decimal MonthlyDepositMaximum;
        public readonly decimal MonthlyWithdrawMaximum;
        private readonly int FreeTrialDurationDays;
        private readonly decimal FeeAmountPercentage;
        private readonly decimal MinimumFeeAmount;

        public UserWalletService(
            IUnitOfWork unitOfWork,

            IBankRepository bankRepository,
            IBankAccountRepository bankAccountRepository,
            IBankTransactionRepository bankTransactionRepository,
            IFeeTransactionRepository feeTransactionRepository,
            ITransactionRepository transactionRepository,
            IUserRepository userRepository,
            IWalletTransferTransactionRepository walletTransferTransactionRepository,

            BankServiceFactory bankServiceFactory,

            decimal monthlyDepositMaximum,
            decimal monthlyWithdrawMaximum,
            int freeTrialDurationDays,
            decimal feeAmountPercentage,
            decimal minimumFeeAmount
        )
        {
            UnitOfWork = unitOfWork;

            BankRepository = bankRepository;
            BankAccountRepository = bankAccountRepository;
            BankTransactionRepository = bankTransactionRepository;
            TransactionRepository = transactionRepository;
            UserRepository = userRepository;
            WalletTransferTransactionRepository = walletTransferTransactionRepository;

            BankServiceFactory = bankServiceFactory;

            MonthlyDepositMaximum = monthlyDepositMaximum;
            MonthlyWithdrawMaximum = monthlyWithdrawMaximum;
            FreeTrialDurationDays = freeTrialDurationDays;
            FeeAmountPercentage = feeAmountPercentage;
            MinimumFeeAmount = minimumFeeAmount;
        }

        // Constructors

        // Methods

        /// <exception cref="ArgumentException">
        ///     Invalid bank account number.
        ///     Invalid Pin.
        ///     User was not found.
        ///     Bank was not found.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     User's wallet is locked.
        ///     User already owns a BankAccount with the same bankAccountNumber in given Bank.
        /// </exception>
        public async Task AddBankAccount(Guid userId, BankType bankType, string bankAccountNumber, string pin)
        {
            var bankAccount = new BankAccount(bankType, userId, bankAccountNumber, pin);
            await UnitOfWork.StartTransactionAsync();
            User user = await GetUnlockedUserAsync(userId);
            bool bankExists = await BankRepository.ExistsAsync(bankType);
            if (!bankExists)
            {
                throw new ArgumentException($"{nameof(Bank)} \"{bankType}\" was not found.", nameof(bankType));
            }
            bool bankAccountExists = await BankAccountRepository.ExistsAsync(userId, bankType, bankAccountNumber);
            if (bankAccountExists)
            {
                throw new InvalidOperationException($"{nameof(User)} (\"{userId}\") already owns a {nameof(BankAccount)} with {nameof(BankAccount.Number)} = \"{bankAccountNumber}\" in given {nameof(Bank)} (\"{bankType}\").");
            }
            await BankAccountRepository.InsertAsync(bankAccount);
            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        /// <exception cref="ArgumentException">
        ///     User was not found.
        ///     BankAccount was not found.
        ///     Invalid pin.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     User is not the owner of bank account.
        ///     Insufficient funds on bank account.
        /// </exception>
        public async Task WithdrawFromBankAccountAsync(Guid userId, Guid bankAccountId, decimal amount)
        {
            await UnitOfWork.StartTransactionAsync();
            (User user, BankAccount bankAccount) = await GetUnlockedUserAndBankAccountAsync(userId, bankAccountId);
            IBankService bankService = BankServiceFactory.GetBankService(bankAccount.BankId);
            await DepositAsync(user, amount);
            var bankTransaction = new BankTransaction(bankAccount, TransactionType.DepositFromBank, amount);
            await BankTransactionRepository.InsertAsync(bankTransaction);
            await bankService.WithdrawAsync(user.UniqueMasterCitizenNumber, bankAccount.Pin, amount);
            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        /// <exception cref="ArgumentException">
        ///     User was not found.
        ///     BankAccount was not found.
        ///     Invalid pin.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     User is not the owner of bank account.
        ///     Insufficient funds on wallet.
        /// </exception>
        public async Task DepositToBankAccountAsync(Guid userId, Guid bankAccountId, decimal amount)
        {
            await UnitOfWork.StartTransactionAsync();
            (User user, BankAccount bankAccount) = await GetUnlockedUserAndBankAccountAsync(userId, bankAccountId);
            IBankService bankService = BankServiceFactory.GetBankService(bankAccount.BankId);
            await WithdrawAsync(user, amount);
            var bankTransaction = new BankTransaction(bankAccount, TransactionType.WithdrawToBank, amount);
            await BankTransactionRepository.InsertAsync(bankTransaction);
            await bankService.DepositAsync(user.UniqueMasterCitizenNumber, bankAccount.Pin, amount);
            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        /// <exception cref="ArgumentException">User not found.</exception>
        public async Task<decimal> GetBalanceAsync(Guid userId)
        {
            User user = await GetUserAsync(userId);
            decimal balance = user.WalletBalance;
            return balance;
        }

        public async Task<ICollection<Transaction>> GetTransactionsAsync(Guid userId)
        {
            ICollection<Transaction> transactions = await TransactionRepository.GetManyByUserIdAsync(userId);
            return transactions;
        }

        public async Task TransferFundsToUserAsync(Guid userId, Guid destinationUserId, decimal amount, string reason)
        {
            await UnitOfWork.StartTransactionAsync();

            User user = await GetUnlockedUserAsync(userId);
            User destinationUser = await GetUnlockedUserAsync(destinationUserId);

            decimal feeAmount = await CalculateFeeAmountAsync(user, amount);

            await WithdrawAsync(user, amount + feeAmount);
            await DepositAsync(destinationUser, amount);

            FeeTransaction feeTransaction = feeAmount > 0 
                ? new FeeTransaction(userId, feeAmount) 
                : null;
            var walletTransferTransaction = new WalletTransferTransaction(userId, destinationUserId, amount, reason, feeTransaction);
            await WalletTransferTransactionRepository.InsertAsync(walletTransferTransaction);

            await UnitOfWork.SaveChangesAndCommitTransactionAsync();
        }

        // Helper methods

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

        /// <exception cref="ArgumentException">
        ///     User was not found.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     User's wallet is locked.
        /// </exception>
        private async Task<User> GetUnlockedUserAsync(Guid userId)
        {
            User user = await GetUserAsync(userId);
            if (user.IsWalletLocked)
            {
                throw new InvalidOperationException($"{nameof(User)}'s (\"{userId}\") wallet is locked");
            }
            return user;
        }

        /// <exception cref="ArgumentException">
        ///     User was not found.
        ///     BankAccount was not found.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     User's wallet is locked.
        ///     User is not the owner of bank account.
        /// </exception>
        private async Task<(User, BankAccount)> GetUnlockedUserAndBankAccountAsync(Guid userId, Guid bankAccountId)
        {
            User user = await GetUserAsync(userId);
            BankAccount bankAccount = await BankAccountRepository.GetByIdAsync(bankAccountId);
            if (bankAccount == null)
            {
                throw new ArgumentException($"{nameof(BankAccount)} \"{bankAccountId}\" was not found.");
            }
            if (bankAccount.UserId != userId)
            {
                throw new InvalidOperationException($"{nameof(User)} \"{userId}\" is not the owner of {nameof(BankAccount)} \"{bankAccountId}\"");
            }
            return (user, bankAccount);
        }

        private async Task DepositAsync(User user, decimal amount)
        {
            decimal monthlyDepositTotal = await TransactionRepository.GetUserMonthlyDepositSumAsync(user.Id);
            decimal allowedDepositAmount = MonthlyDepositMaximum - monthlyDepositTotal;
            if (amount > allowedDepositAmount)
            {
                throw new InvalidOperationException($"Deposit would go over monthly deposit limit. Allowed deposit amount is {allowedDepositAmount}.");
            }
            user.Deposit(amount);
        }

        private async Task WithdrawAsync(User user, decimal amount)
        {
            decimal monthlyWithdrawTotal = await TransactionRepository.GetUserMonthlyWithdrawTotalAsync(user.Id);
            decimal allowedWithdrawAmount = MonthlyWithdrawMaximum - monthlyWithdrawTotal;
            if (amount > allowedWithdrawAmount)
            {
                throw new InvalidOperationException($"Withdraw would go over monthly withdraw limit. Allowed withdraw amount is {allowedWithdrawAmount}.");
            }
            user.Deposit(amount);
        }

        private async Task<decimal> CalculateFeeAmountAsync(User sourceUser, decimal amount)
        {
            DateTime now = DateTime.Now;

            TimeSpan sourceUserAge = now - sourceUser.CreationDateTime;
            if (TimeSpan.Zero <= sourceUserAge && sourceUserAge <= TimeSpan.FromDays(FreeTrialDurationDays))
            {
                return 0;
            }

            WalletTransferTransaction lastWalletTransferTransaction = await WalletTransferTransactionRepository.GetLastUserWalletTransferTransactionAsync(sourceUser.Id);
            if (lastWalletTransferTransaction == null || !(lastWalletTransferTransaction.DateTime.Year == now.Year && lastWalletTransferTransaction.DateTime.Month == now.Month))
            {
                return 0;
            }

            decimal feeAmount = Math.Max(amount * FeeAmountPercentage / 100, MinimumFeeAmount);
            return feeAmount;
        }
    }
}
