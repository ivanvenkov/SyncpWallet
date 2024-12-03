using System;

namespace SyncpWallet.Repositories.DTOs
{
    public class TransactionDetailsDto
    {
        public TransactionDetailsDto(int id, int walletId, decimal amount, string transactionType, DateTime created)
        {
            Id = id;
            WalletId = walletId;
            Amount = amount;
            TransactionType = transactionType;
            Created = created;
        }
       
        public int Id { get; }
        public int WalletId { get; }
        public decimal Amount { get; }
        public string TransactionType { get; }
        public DateTime Created { get;  }
    }
}
