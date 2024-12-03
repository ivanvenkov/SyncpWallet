using SyncpWallet.Repositories.DTOs;
using SyncpWallet.Repositories.Interfaces;
using SyncpWallet.Services.Interfaces;
using SyncpWallet.Services.Models.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace SyncpWallet.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }
        public async Task<TransactionDetailsModel> AddExpenseAsync(TransactionCreateModel model, CancellationToken ct)
        {
            var dto = new TransactionCreateDto(model.WalletId, model.Amount, "Expense");
            var (transactionDto, errorMessage) = await this.transactionRepository.AddTransactionAsync(dto, ct);

            if (transactionDto == null)
            {
                return new TransactionDetailsModel
                {
                    IsSuccess = false,
                    ErrorMessage = errorMessage
                };
            }

            return new TransactionDetailsModel(
                transactionDto.Id,
                transactionDto.WalletId,
                transactionDto.Amount,
                transactionDto.TransactionType,
                true,
                transactionDto.Created);
        }

        public async Task<TransactionDetailsModel> AddIncomeAsync(TransactionCreateModel model, CancellationToken ct)
        {
            var dto = new TransactionCreateDto(model.WalletId, model.Amount, "Income");
            var (transactionDto, errorMessage) = await this.transactionRepository.AddTransactionAsync(dto, ct);

            if (transactionDto == null)
            {
                return new TransactionDetailsModel
                {
                    IsSuccess = false,
                    ErrorMessage = errorMessage
                };
            }

            return new TransactionDetailsModel(
                transactionDto.Id,
                transactionDto.WalletId,
                transactionDto.Amount,
                transactionDto.TransactionType, 
                true, 
                transactionDto.Created);
        }
    }
}
