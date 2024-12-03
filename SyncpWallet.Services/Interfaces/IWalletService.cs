using SyncpWallet.Services.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncpWallet.Services.Interfaces
{
    public interface IWalletService
    {
        Task<WalletDetailsModel> CreateWalletAsync(WalletCreateModel model, CancellationToken ct);
        Task<IEnumerable<WalletDetailsModel>> GetWalletsAsync(int userId, CancellationToken ct);
        Task<int> DeleteWalletAsync(int walletId, CancellationToken ct);
    }
}