using Autofac;
using Autofac.Integration.WebApi;
using FluentValidation;
using SyncpWallet.ExceptionsHandler;
using SyncpWallet.Repositories;
using SyncpWallet.Repositories.Interfaces;
using SyncpWallet.Services;
using SyncpWallet.Services.Interfaces;
using SyncpWallet.Services.Models;
using SyncpWallet.Services.Models.Transactions;
using SyncpWallet.Validators;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SyncpWallet
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.EnsureInitialized();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AsClosedTypesOf(typeof(IValidator<>))
                   .AsImplementedInterfaces();

            builder.RegisterType<DatabaseConnector>()
                    .AsSelf()
                    .InstancePerLifetimeScope();

            builder.RegisterType<WalletService>().As<IWalletService>().InstancePerRequest();
            builder.RegisterType<WalletRepository>().As<IWalletRepository>().InstancePerRequest(); 
            builder.RegisterType<TransactionService>().As<ITransactionService>().InstancePerRequest();
            builder.RegisterType<TransactionRepository>().As<ITransactionRepository>().InstancePerRequest();
            builder.RegisterType<WalletCreateModelValidator>().As<IValidator<WalletCreateModel>>().InstancePerRequest();
            builder.RegisterType<TransactionCreateModelValidator>().As<IValidator<TransactionCreateModel>>().InstancePerRequest();

            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configure(config =>
            {
                config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            });
        }
    }
}
