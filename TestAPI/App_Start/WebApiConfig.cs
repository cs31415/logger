using System.Web.Http;
using logger.Middleware;
using Logger.Managers;
using TestApi;

namespace TestAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ILoggerFactory loggerFactory = (ILoggerFactory)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILoggerFactory));
            var requestLogger = new AccessLoggingHandler(loggerFactory, LoggerTypes.AccessLog.ToString());
            config.MessageHandlers.Add(requestLogger);
        }
    }
}
