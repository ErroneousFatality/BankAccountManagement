using Domain.Common;
using Domain.Entities.BankAccounts;
using Domain.Entities.Transactions;
using Domain.Entities.WalletTransferTransactions;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Users
{
    public class User
    {
        // Properties
        public Guid Id { get; private set; }
        public string UniqueMasterCitizenNumber { get; private set; }
        public DateTime CreationDateTime { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        
        public decimal Balance { get; private set; }
        public ICollection<BankAccount> BankAccounts { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; }
        public ICollection<WalletTransferTransaction> WalletTransferDepositTransactions { get; private set; }

        // Computed
        public string FullName => $"{FirstName} {LastName}";

        // Constructors

        /// <exception cref="ArgumentException">
        ///     First name
        ///     Last name
        ///     All characters must be digits.
        ///     Length must be {UniqueMasterCitizenNumberLength}.
        ///     Control digit is invalid.
        /// </exception>
        public User(
            string uniqueMasterCitizenNumber,
            string firstName,
            string lastName
        )
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < FirstNameMinLength || firstName.Length > FirstNameMaxLength)
            {
                throw new ArgumentException($"First name is mandatory and its length must be between [{FirstNameMinLength}, {FirstNameMaxLength}]");
            }
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < LastNameMinLength || lastName.Length > LastNameMaxLength)
            {
                throw new ArgumentException($"Last name is mandatory and its length must be between [{LastNameMinLength}, {LastNameMaxLength}]");
            }
            Utils.ValidateUniqueMasterCitizenNumber(uniqueMasterCitizenNumber);
            UniqueMasterCitizenNumber = uniqueMasterCitizenNumber;
            CreationDateTime = DateTime.Now;
            FirstName = firstName;
            LastName = lastName;
        }

        private User() {
            BankAccounts = new List<BankAccount>();
            Transactions = new List<Transaction>();
            WalletTransferDepositTransactions = new List<WalletTransferTransaction>();
        }

        // Methods

        internal void Deposit(decimal amount)
        {
            Balance += amount;
        }

        /// <exception cref="InvalidOperationException">Insufficient funds.</exception>
        internal void Withdraw(decimal amount)
        {
            Balance -= amount;
            if (Balance < 0)
            {
                throw new InvalidOperationException($"{nameof(User)} \"{Id}\" doesn't have enough funds in his wallet to {nameof(Withdraw)}.");
            }
        }

        // Constants
        public const int UniqueMasterCitizenNumberLength = Utils.UniqueMasterCitizenNumberLength;

        public const int FirstNameMinLength = 3;
        public const int FirstNameMaxLength = 30;

        public const int LastNameMinLength = 3;
        public const int LastNameMaxLength = 30;
    }
}
