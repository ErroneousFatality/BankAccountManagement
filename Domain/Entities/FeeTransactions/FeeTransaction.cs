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
            WalletTransferTransaction walletTransferTransaction,
            decimal amount
        )
            : base(
                  walletTransferTransaction.UserId,
                  TransactionType.Fee,
                  amount
            )
        {
            WalletTransferTransaction = walletTransferTransaction;
            WalletTransferTransactionId = walletTransferTransaction.Id;
        }

        private FeeTransaction() { }
    }
}