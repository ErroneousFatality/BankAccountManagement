using Domain.Entities.FeeTransactions;
using Domain.Entities.Transactions;
using Domain.Entities.Users;
using System;

namespace Domain.Entities.WalletTransferTransactions
{
    public class WalletTransferTransaction : Transaction
    {
        // Properties
        public Guid DestinationUserId { get; private set; }
        public User DestinationUser { get; private set; }

        public string Reason { get; private set; }

        public Guid? FeeTransactionId { get; private set; }
        public FeeTransaction FeeTransaction { get; private set; }

        // Constructors

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Amount must be a positive decimal value.
        ///     Maximum length of reason.
        /// </exception>
        public WalletTransferTransaction(
            Guid userId,
            Guid destinationUserId,
            decimal amount,
            string reason,
            FeeTransaction feeTransaction = null
        )
            : base(
                  userId,
                  TransactionType.WalletTransfer,
                  amount
            )
        {
            if (reason != null && reason.Length > ReasonMaxLength)
            {
                throw new ArgumentException($"Maximum length of {nameof(reason)} is {ReasonMaxLength}.");
            }
            DestinationUserId = destinationUserId;
            Reason = reason;
            FeeTransaction = feeTransaction;
        }

        private WalletTransferTransaction() { }

        // Constants
        public const int ReasonMaxLength = 600;
    }
}