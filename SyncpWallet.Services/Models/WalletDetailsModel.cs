namespace SyncpWallet.Services.Models
{
    public class WalletDetailsModel
    {
        public WalletDetailsModel(int id, string name, string currency, decimal? amount, int userId)
        {
            Id = id;
            Name = name;
            Currency = currency;
            Amount = amount;
            UserId = userId;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public int UserId { get; set; }
    }
}