using SyncpWallet.Services.Interfaces;
using SyncpWallet.Services.Models.Transactions;
using SyncpWallet.Validators;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SyncpWallet.Controllers
{
    [RoutePrefix("api/v1/Transactions")]
    public class TransactionsController : ApiController
    {
        private readonly TransactionCreateModelValidator validator;
        private readonly ITransactionService transactionService;

        public TransactionsController(ITransactionService transactionService, TransactionCreateModelValidator validator)
        {
            this.transactionService = transactionService;
            this.validator = validator;
        }

        [HttpPost]
        [Route("add-income")]
        public async Task<IHttpActionResult> AddIncome([FromBody]TransactionCreateModel model, CancellationToken ct)
        {
            var validationResult = await this.validator.ValidateAsync(model, ct);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(string.Join(",", errors));
            }

            var result = await this.transactionService.AddIncomeAsync(model, ct);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost]
        [Route("add-expense")]
        public async Task<IHttpActionResult> AddExpense([FromBody] TransactionCreateModel model, CancellationToken ct)
        {
            var validationResult = await this.validator.ValidateAsync(model, ct);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(string.Join(",", errors));
            }

            var result = await this.transactionService.AddExpenseAsync(model, ct);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
