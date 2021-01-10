using System;
using System.Web;
using System.Web.Http;
using Logger.Managers;
using TestApi;
using TestApi.Builders;
using TestApi.Helpers;
using Unity;
using Unity.WebApi;

namespace TestAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            // Can obtain these from web.config 
            var application = "TestAPI";
            var applicationVersion = "1.0";
            var environment = "Dev";

            var loggerFactory = LoggerFactory.GetLoggerFactoryInstance(System.Enum.GetNames(typeof(LoggerTypes)), application,applicationVersion, environment, LoggerTypes.AccessLog.ToString());
            container.RegisterInstance<ILoggerFactory>(loggerFactory, InstanceLifetime.Singleton);
            container.RegisterInstance<IApplicationLogRecordBuilder>(new ApplicationLogRecordBuilder(application, applicationVersion, environment));
            container.RegisterType<IStructuredLogHelper, StructuredLogHelper>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}