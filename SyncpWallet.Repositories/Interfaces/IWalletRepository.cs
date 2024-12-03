using SyncpWallet.Repositories.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncpWallet.Repositories.Interfaces
{
    public interface IWalletRepository
    {
        Task<WalletDetailsModelDto> CreateWalletAsync(WalletCreateModelDto model, CancellationToken ct);
        Task<IEnumerable<WalletDetailsModelDto>> GetWalletsByUserIdAsync(int userId, CancellationToken ct);
        Task<int> DeleteWalletAsync(int walletId, CancellationToken cancellationToken);
        Task<bool> DoesWalletExistAsync(string walletName, int userId, CancellationToken cancellationToken);
    }
}