using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using GameWebApi.Common.Security;
using GameWebApi.Data;
using GameWebApi.MaintenanceProcessing;

namespace GameWebApi
{
    /// <summary>
    /// This is building a container that includes all the dependencies of the different classes in the application
    /// https://docs.asp.net/en/latest/fundamentals/dependency-injection.html
    /// </summary>
    public class AutofacConfigurator
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            //This line registers all the classes that derive from ApiController class
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterApiControllers(assembly);

            //This registers the MaintenanceProcessors as the interface they implement (IPlayerMaintenanceProcessor etc)
            builder.RegisterType<PlayerMaintenanceProcessor>().As<IPlayerMaintenanceProcessor>();
            builder.RegisterType<ItemsMaintenanceProcessor>().As<IItemsMaintenanceProcessor>();

            builder.RegisterType<MongoDbRepository>().As<IRepository>().SingleInstance();

            builder.RegisterType<BasicSecurityService>().As<IBasicSecurityService>().SingleInstance();

            IContainer container = builder.Build();
            return container;
        }
    }
}