namespace SyncpWallet.Repositories.DTOs
{
    public class TransactionCreateDto
    {
        public TransactionCreateDto(int walletId, decimal amount, string transactionType)
        {
            WalletId = walletId;
            Amount = amount;
            TransactionType = transactionType;
        }

        public int WalletId { get; }
        public decimal Amount { get; }
        public string TransactionType { get; }
    }
}
