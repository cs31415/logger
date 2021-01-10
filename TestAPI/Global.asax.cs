using System;
using System.Web;
using System.Web.Http;
using Logger.Middleware;
using TestApi.Helpers;

namespace TestAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            // Setup unhandled error logging & filter
            var accessLoggingModule = HttpContext.Current.ApplicationInstance.Modules.Get("AccessLoggingModule") as AccessLoggingModule;
            if (accessLoggingModule != null)
            {
                accessLoggingModule.LogUnhandledError = LogUnhandledError;
                accessLoggingModule.Filter = Filter;
            }
        }

        private void LogUnhandledError(Exception ex, string correlationId)
        {
            var structuredLogHelper =
                GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IStructuredLogHelper)) as IStructuredLogHelper;
            structuredLogHelper?.LogError(ex, correlationId);
        }

        private bool Filter(HttpRequest request)
        {
            return request.Url.AbsolutePath.Contains("/health");
        }
    }
}
