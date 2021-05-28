using Domain.Entities.Users;
using System;

namespace Domain.Entities.Transactions
{
    public abstract class Transaction
    {
        // Properties
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public DateTime DateTime { get; private set; }
        public TransactionType Type { get; private set; }
        public PaymentDirectionType PaymentDirection { get; private set; }
        public decimal Amount { get; private set; }


        // Constructors

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid type.
        ///     amount must be a positive decimal value.
        /// </exception>
        protected Transaction(
            Guid userId,
            TransactionType type, 
            decimal amount
        )
        {
            if (!Enum.IsDefined(type))
            {
                throw new ArgumentOutOfRangeException(nameof(type), $"Invalid {nameof(type)}.");
            }
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), $"{nameof(amount)} must be a positive decimal value.");
            }
            UserId = userId;
            DateTime = DateTime.Now;
            Type = type;
            PaymentDirection = type == TransactionType.DepositFromBank 
                ? PaymentDirectionType.Deposit 
                : PaymentDirectionType.Withdraw;
            Amount = amount;
        }

        protected Transaction() { }
    }
}