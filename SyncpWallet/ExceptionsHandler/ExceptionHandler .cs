using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace SyncpWallet.ExceptionsHandler
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private const string WalletNotFoundMessage = "Wallet not found.";
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken ct)
        {
            var exception = context.Exception;
            int statusCode;
            string title;
            string detail;

            if (exception is SqlException ex)
            {                
                statusCode = ex.Message == WalletNotFoundMessage ?
                    (int)HttpStatusCode.NotFound:
                    (int)HttpStatusCode.BadRequest; 
                title = exception.Message;
                detail = HttpContext.Current.IsDebuggingEnabled ? exception.StackTrace : null;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                title = exception.Message;
                detail = HttpContext.Current.IsDebuggingEnabled ? exception.StackTrace : null;
            }

            var response = new
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            context.Result = new ResponseMessageResult(
            context.Request.CreateResponse(
                (HttpStatusCode)statusCode,
                response,
                GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            );
        }
    }
}
