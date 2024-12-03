using SyncpWallet.Repositories.DTOs;
using SyncpWallet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SyncpWallet.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly DatabaseConnector dbConnector;

        public WalletRepository(DatabaseConnector dbConnector)
        {
            this.dbConnector = dbConnector;
        }

        public async Task<WalletDetailsModelDto> CreateWalletAsync(WalletCreateModelDto model, CancellationToken ct)
        {
            using (var reader = await this.dbConnector.ExecReaderAsyncTask("usp_WalletInsert", null,
                        new SqlParameter("@name", model.Name),
                        new SqlParameter("@amount", model.Amount),
                        new SqlParameter("@currency", model.Currency),
                        new SqlParameter("@userId", model.UserId)))
            {
                if (!reader.Read())
                {
                    throw new Exception("Inserting new Wallet failed");
                }
                var id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WalletId")));
                return new WalletDetailsModelDto(id, model.Name, model.Currency, model.Amount, model.UserId);
            }
        }

        public async Task<IEnumerable<WalletDetailsModelDto>> GetWalletsByUserIdAsync(int userId, CancellationToken ct)
        {
            var wallets = new List<WalletDetailsModelDto>();

            using (var reader = await this.dbConnector.ExecReaderAsyncTask("usp_GetWalletsByUserId", null,
                    new SqlParameter("@UserId", userId)))
            {
                var tasks = new List<Task>();

                while (await reader.ReadAsync(ct))
                {
                    var id = reader.GetInt32(reader.GetOrdinal("Id"));
                    var name = reader.GetString(reader.GetOrdinal("Name"));
                    var currency = reader.GetString(reader.GetOrdinal("Currency"));
                    var amount = reader.GetDecimal(reader.GetOrdinal("Amount"));
                    var user = reader.GetInt32(reader.GetOrdinal("UserId"));
                    var wallet = new WalletDetailsModelDto(id, name, currency, amount, user);

                    lock (wallets)
                    {
                        wallets.Add(wallet);
                    }

                    await Task.WhenAll(tasks);
                }
            }
            return wallets;
        }

        public async Task<int> DeleteWalletAsync(int walletId, CancellationToken cancellationToken)
        {
            return await this.dbConnector.ExecNonQueryAsyncTask(
                "usp_DeleteWallet", null,
                new SqlParameter("@walletId", walletId));
        }

        public async Task<bool> DoesWalletExistAsync(string walletName,int userId, CancellationToken cancellationToken)
        {
            using (var reader = await this.dbConnector.ExecReaderAsyncTask(
                "usp_CheckWalletNameForDuplicates",
                null,
                new SqlParameter("@UserId", userId),
                new SqlParameter("@WalletName", walletName)))
            {
                return await reader.ReadAsync(cancellationToken);
            }
        }
    }
}

