using FluentValidation.WebApi;
using System.Web.Http;

namespace SyncpWallet
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {        
            // Web API routes
            config.MapHttpAttributeRoutes();

            FluentValidationModelValidatorProvider.Configure(config);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }   
}
