using SyncpWallet.Repositories.DTOs;
using SyncpWallet.Repositories.Interfaces;
using SyncpWallet.Services.Interfaces;
using SyncpWallet.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncpWallet.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            this.walletRepository = walletRepository;
        }

        public async Task<WalletDetailsModel> CreateWalletAsync(WalletCreateModel model, CancellationToken ct)
        {
            var dto = new WalletCreateModelDto(model.Name, model.Amount, model.Currency, model.UserId);

            var resultDto = await this.walletRepository.CreateWalletAsync(dto, ct);
            var result = new WalletDetailsModel(resultDto.Id, resultDto.Name, resultDto.Currency,resultDto.Amount, resultDto.UserId );
            return result;
        }

        public async Task<IEnumerable<WalletDetailsModel>> GetWalletsAsync(int userId, CancellationToken ct)
        {
            var resultDtos = await this.walletRepository.GetWalletsByUserIdAsync(userId, ct);
            var result = resultDtos.Select(x=> new WalletDetailsModel(x.Id, x.Name, x.Currency, x.Amount , x.UserId));
            return result;
        }

        public async Task<int> DeleteWalletAsync(int walletId, CancellationToken ct)
        {
            return await this.walletRepository.DeleteWalletAsync(walletId, ct);
        }
    }
}
