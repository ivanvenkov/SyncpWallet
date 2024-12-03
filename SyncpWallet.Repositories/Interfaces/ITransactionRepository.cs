using SyncpWallet.Repositories.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace SyncpWallet.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<(TransactionDetailsDto dto, string errorMessage)> AddTransactionAsync(TransactionCreateDto model, CancellationToken ct);
    }
}