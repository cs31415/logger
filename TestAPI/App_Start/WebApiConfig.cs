using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Logger.Middleware;
using TestApi.Helpers;

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

            // Register exception logging handler
            var exceptionLoggingHandler = new ExceptionLoggingHandler(LogUnhandledError);
            config.MessageHandlers.Add(exceptionLoggingHandler);
        }

        private static void LogUnhandledError(string errorMessage, string correlationId)
        {
            var structuredLogHelper =
                GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IStructuredLogHelper)) as IStructuredLogHelper;
            structuredLogHelper?.LogError(errorMessage, correlationId);
        }
    }
}
