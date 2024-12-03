

namespace SyncpWallet.Services.Models
{
    public class WalletCreateModel
    {
        public string Name { get; set; }        
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public int UserId { get; set; }
    }
}