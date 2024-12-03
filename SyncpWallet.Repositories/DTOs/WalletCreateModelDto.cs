namespace SyncpWallet.Repositories.DTOs
{
    public class WalletCreateModelDto
    {
        public WalletCreateModelDto(string name, decimal? amount, string currency, int userId)
        {
            Name = name;
            Amount = amount;
            Currency = currency;
            UserId = userId;
        }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public int UserId { get; set; }
    }
}
