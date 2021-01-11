using System;
using System.Diagnostics;
using System.Web;
using Logger.Helpers;
using Logger.Managers;

namespace Logger.Middleware
{
    /// <summary>
    /// Http module to log access record and unhandled errors
    /// </summary>
    public class AccessLoggingModule: IHttpModule
    {
        private readonly Stopwatch _timer = new Stopwatch();
        
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
        }

        public void Dispose()
        {
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            _timer.Start();
        }
        
        private void EndRequest(object sender, EventArgs e)
        {
            _timer.Stop();
            var request = HttpContext.Current.Request;
            var filter = HttpContext.Current.Application.Get("Filter") as Func<HttpRequest, bool>;
            var filterRequest = filter != null && filter(request);
            if (!filterRequest)
            {
                var response = HttpContext.Current.Response;
                var correlationId = (new HttpUtils().GetCorrelationId(request));

                var responseTimeMs = _timer.ElapsedMilliseconds;

                var loggerFactory = LoggerFactory.Instance;
                var builder = loggerFactory.GetAccessLogRecordBuilder();
                var logger = loggerFactory.GetLogger(LoggerFactory.AccessLogName);
                logger.LogInfo(builder.Build(string.Empty, request, response, responseTimeMs, correlationId));
            }
        }
    }
}
