using System;
using System.Web;
using System.Web.Http;
using Logger.Helpers;
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

            // Setup access log filter
            var context = HttpContext.Current.ApplicationInstance;
            Func<HttpRequest, bool> filter = Filter;
            context.Application.Add("Filter", filter);
        }

        protected void Application_Error()
        {
            try
            {
                var ex = HttpContext.Current.Server.GetLastError();
                var request = HttpContext.Current.Request;
                var correlationId = (new HttpUtils().GetCorrelationId(request));
                LogUnhandledError(ex, correlationId);
            }
            catch
            {
                // ignored
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
