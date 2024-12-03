using FluentValidation;
using SyncpWallet.Services.Models.Transactions;

namespace SyncpWallet.Validators
{
    public class TransactionCreateModelValidator : AbstractValidator<TransactionCreateModel>
    {
        public TransactionCreateModelValidator()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty()
                .WithMessage("Wallet id is required.");

            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage("Amount is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount should be greater than 0.");

        }
    }
}
