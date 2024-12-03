using FluentValidation;
using SyncpWallet.Services.Models;
using System.Threading.Tasks;
using System.Threading;
using SyncpWallet.Repositories.Interfaces;

namespace SyncpWallet.Validators
{
    public class WalletCreateModelValidator : AbstractValidator<WalletCreateModel>
    {
        private readonly IWalletRepository walletRepository;
        public WalletCreateModelValidator(IWalletRepository walletRepository)
        {
            this.walletRepository = walletRepository;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MustAsync(async (model, walletName, ct) =>
            {
                return await IsUniqueWalletName(walletName, model.UserId, ct);
            }).WithMessage("A wallet with the same name already exists for this user.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User id is required.");
            RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required.");
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Balance must be 0 or greater.");
            RuleFor(x => x.Currency).NotEmpty().WithMessage("Name is required.");
        }

        private async Task<bool> IsUniqueWalletName(string walletName, int userId, CancellationToken ct)
        {
            return !await this.walletRepository.DoesWalletExistAsync(walletName, userId, ct);
        }
    }
}
