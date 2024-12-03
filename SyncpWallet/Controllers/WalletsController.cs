using SyncpWallet.Services.Interfaces;
using SyncpWallet.Services.Models;
using SyncpWallet.Validators;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SyncpWallet.Controllers
{
    [RoutePrefix("api/v1/Wallets")]
    public class WalletsController : ApiController
    {
        private readonly IWalletService walletService;
        private readonly WalletCreateModelValidator validator;

        public WalletsController(IWalletService walletService, WalletCreateModelValidator validator)
        {
            this.walletService = walletService;
            this.validator = validator;
        }

        [HttpPost]
        [Route("create-wallet")]
        public async Task<IHttpActionResult> Post([FromBody] WalletCreateModel model, CancellationToken ct)
        {
            var validationResult = await this.validator.ValidateAsync(model, ct);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(string.Join(",", errors));
            }

            var response = await this.walletService.CreateWalletAsync(model, ct);
            return Ok(response);
        }


        [HttpGet]
        [Route("wallets-by-userId/{userId}")]
        public async Task<IHttpActionResult> Get(int userId, CancellationToken ct)
        {            
            var response = await this.walletService.GetWalletsAsync(userId, ct);
            if (response == null || !response.Any())
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("{walletId}")]
        public async Task<IHttpActionResult> Delete(int walletId, CancellationToken ct)
        {
            var result = await this.walletService.DeleteWalletAsync(walletId, ct);
            if (result == 0)
            {
                return NotFound();
            }

            return Ok(new { DeletedWalletId = walletId });
        }
    }
}