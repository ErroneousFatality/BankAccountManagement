namespace Domain.Entities.Transactions
{
    public enum TransactionType : byte
    {
        DepositFromBank = 1,
        WithdrawToBank = 2,
        WalletTransfer = 3,
        Fee = 4
    }
}
