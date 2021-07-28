using Domain.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services.External
{
    public class MockBankService : IBankService
    {
        // Fields
        private static readonly IDictionary<string, MockBankAccount> BankAccountByUniqueMasterCitizenNumber;

        // Constructors
        static MockBankService()
        {
            BankAccountByUniqueMasterCitizenNumber = new Dictionary<string, MockBankAccount>();
        }

        public MockBankService()
        {
   
        }
        // Methods

        /// <exception cref="ArgumentException">Invalid pin.</exception>
        public Task<decimal> CheckBalanceAsync(string uniqueMasterCitizenNumber, string pin)
        {
            MockBankAccount bankAccount = GetOrCreateBankAccount(uniqueMasterCitizenNumber, pin);
            decimal balance = bankAccount.Balance;
            return Task.FromResult(balance);
        }

        /// <exception cref="ArgumentException">Invalid pin.</exception>
        public Task DepositAsync(string uniqueMasterCitizenNumber, string pin, decimal amount)
        {
            MockBankAccount bankAccount = GetOrCreateBankAccount(uniqueMasterCitizenNumber, pin);
            bankAccount.Deposit(amount);
            return Task.CompletedTask;
        }

        /// <exception cref="ArgumentException">Invalid pin.</exception>
        /// <exception cref="InvalidOperationException">Insufficient funds.</exception>
        public Task WithdrawAsync(string uniqueMasterCitizenNumber, string pin, decimal amount)
        {
            MockBankAccount bankAccount = GetOrCreateBankAccount(uniqueMasterCitizenNumber, pin);
            bankAccount.Withdraw(amount);
            return Task.CompletedTask;
        }

        // Helper methods

        /// <exception cref="ArgumentException">Invalid pin.</exception>
        private static MockBankAccount GetOrCreateBankAccount(string uniqueMasterCitizenNumber, string pin)
        {
            if (BankAccountByUniqueMasterCitizenNumber.TryGetValue(uniqueMasterCitizenNumber, out MockBankAccount bankAccount))
            {
                if (bankAccount.Pin != pin)
                {
                    throw new ArgumentException($"Invalid {nameof(pin)}", nameof(pin));
                }
            }
            else
            {
                bankAccount = new MockBankAccount(uniqueMasterCitizenNumber, pin, InitialBalance);
                BankAccountByUniqueMasterCitizenNumber.Add(uniqueMasterCitizenNumber, bankAccount);
            }
            return bankAccount;
        }

        // Constants
        private const decimal InitialBalance = 1000;

        // Classes
        private class MockBankAccount
        {
            // Properties
            public string UniqueMasterCitizenNumber { get; private set; }
            public string Pin { get; private set; }
            public decimal Balance { get; private set; }

            // Constructors

            /// <exception cref="ArgumentException">
            ///     UniqueMasterCitizenNumber - All characters must be digits.
            ///     UniqueMasterCitizenNumber - Length must be {UniqueMasterCitizenNumberLength}.
            ///     UniqueMasterCitizenNumber - Control digit is invalid.
            /// </exception>
            /// <exception cref="ArgumentNullException">pin</exception>
            /// <exception cref="ArgumentOutOfRangeException">Balance can not be a negative number.</exception>
            public MockBankAccount(string uniqueMasterCitizenNumber, string pin, decimal balance = 0)
            {
                Utils.ValidateUniqueMasterCitizenNumber(uniqueMasterCitizenNumber);
                if (string.IsNullOrWhiteSpace(uniqueMasterCitizenNumber))
                {
                    throw new ArgumentNullException(nameof(pin));
                }
                if (balance < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(balance), $"{nameof(Balance)} can not be a negative number.");
                }
                UniqueMasterCitizenNumber = uniqueMasterCitizenNumber;
                Pin = pin;
                Balance = balance;
            }

            // Methods

            /// <exception cref="InvalidOperationException">Insufficient funds.</exception>
            public void Withdraw(decimal amount)
            {
                Balance -= amount;
                if (Balance < 0)
                {
                    throw new InvalidOperationException($"{nameof(MockBankAccount)} \"{UniqueMasterCitizenNumber}\" doesn't have enough funds in his wallet to {nameof(Withdraw)}.");
                }
            }

            public void Deposit(decimal amount)
            {
                Balance += amount;
            }
        }
    }
}
