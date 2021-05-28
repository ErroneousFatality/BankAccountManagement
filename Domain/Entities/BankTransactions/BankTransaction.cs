using Domain.Entities.BankAccounts;
using Domain.Entities.Transactions;
using System;

namespace Domain.Entities.BankTransactions
{
    public class BankTransaction : Transaction
    {
        // Properties
        public Guid BankAccountId { get; private set; }
        public BankAccount BankAccount { get; private set; }

        // Constructors

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid bank transaction type.
        ///     Amount must be a positive decimal value.
        /// </exception>
        public BankTransaction(
            BankAccount bankAccount,
            TransactionType type,
            decimal amount
        )
            : base(
                  bankAccount.UserId,
                  type, 
                  amount
            )
        {
            if (!(type == TransactionType.DepositFromBank || type == TransactionType.WithdrawToBank))
            {
                throw new ArgumentOutOfRangeException(nameof(type), "Invalid bank transaction type.");
            }
            BankAccount = bankAccount;
            BankAccountId = bankAccount.Id;
        }

        private BankTransaction() { }
    }
}