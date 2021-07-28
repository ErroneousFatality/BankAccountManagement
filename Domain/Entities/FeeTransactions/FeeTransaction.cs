using Domain.Entities.Transactions;
using Domain.Entities.WalletTransferTransactions;
using System;

namespace Domain.Entities.FeeTransactions
{
    public class FeeTransaction : Transaction
    {
        // Properties
        public Guid WalletTransferTransactionId { get; private set; }
        public WalletTransferTransaction WalletTransferTransaction { get; private set; }

        // Constructors

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid type.
        ///     Amount must be a positive decimal value.
        /// </exception>
        public FeeTransaction(
            Guid userId,
            decimal amount
        )
            : base(
                  userId,
                  TransactionType.Fee,
                  amount
            )
        {
        }

        private FeeTransaction() { }
    }
}