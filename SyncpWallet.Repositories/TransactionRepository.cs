using SyncpWallet.Repositories.DTOs;
using SyncpWallet.Repositories.Interfaces;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SyncpWallet.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DatabaseConnector dbConnector;

        public TransactionRepository(DatabaseConnector dbConnector)
        {
            this.dbConnector = dbConnector;
        }

        public async Task<(TransactionDetailsDto dto, string errorMessage)> AddTransactionAsync(TransactionCreateDto model, CancellationToken ct)
        {
            using (var reader = await this.dbConnector.ExecReaderAsyncTask
                ("usp_AddTransaction",
                 null,
                 new SqlParameter("@WalletId", model.WalletId),
                 new SqlParameter("@Amount", model.Amount),
                 new SqlParameter("@TransactionType", model.TransactionType)))
            {
                if (!reader.Read())
                {
                    throw new Exception("Transaction failed.");
                }

                var id = reader.GetInt32(reader.GetOrdinal("Id"));
                var walletId = reader.GetInt32(reader.GetOrdinal("WalletId"));
                var amount = reader.GetDecimal(reader.GetOrdinal("Amount"));
                var transactionType = reader.GetString(reader.GetOrdinal("TransactionType"));
                var created = reader.GetDateTime(reader.GetOrdinal("Created"));

                var transactionDto = new TransactionDetailsDto(id, walletId, amount, transactionType, created);
                return (transactionDto, null);
            }
        }
    }
}
