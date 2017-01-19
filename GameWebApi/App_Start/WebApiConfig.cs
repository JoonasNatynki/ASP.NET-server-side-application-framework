using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using GameWebApi.Common.Security;
using GameWebApi.Web.Common;
using log4net.Config;

namespace GameWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Adds filter for validating the models sent to Web API
            config.Filters.Add(new ValidateModelAttribute());

            //Make Web API use our GlobalExceptionLogger for logging exceptions
            config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger());

            //Replace the default exception handler with our own implementation
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            //Configure dependency injection
            IContainer container = AutofacConfigurator.Configure();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.MessageHandlers.Add(new BasicAuthenticationMessageHandler(container.Resolve<IBasicSecurityService>()));

            XmlConfigurator.Configure();
        }
    }
}
