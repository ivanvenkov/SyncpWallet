using System;
using System.Text.Json.Serialization;

namespace SyncpWallet.Services.Models.Transactions
{
    public class TransactionDetailsModel
    {
        public TransactionDetailsModel()
        {                
        }
        public TransactionDetailsModel(int id, int walletId, decimal amount, string transactionType, bool isSuccess, DateTime created)
        {
            Id = id;
            WalletId = walletId;
            Amount = amount;
            TransactionType = transactionType;
            IsSuccess = isSuccess;
            Created = created;
            
        }
        [JsonPropertyName("TransactionId")]
        public int Id { get; }
        public int WalletId { get; }
        public decimal Amount { get; }
        public string TransactionType { get; }
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; } 
        public DateTime Created { get; }

    }
}
