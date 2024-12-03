using SyncpWallet.Services.Models.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace SyncpWallet.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDetailsModel> AddExpenseAsync(TransactionCreateModel model, CancellationToken ct);
        Task<TransactionDetailsModel> AddIncomeAsync(TransactionCreateModel model, CancellationToken ct);
    }
}