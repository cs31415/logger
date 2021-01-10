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
        public Action<Exception, string> LogUnhandledError { get; set; }
        private readonly Stopwatch _timer = new Stopwatch();
        
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
            context.Error += Context_Error;
        }

        public Func<HttpRequest, bool> Filter { get; set; }

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
            var filterRequest = Filter != null && !Filter(request);
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
        
        private void Context_Error(object sender, EventArgs e)
        {
            try 
            {
                if (LogUnhandledError != null)
                {
                    var ex = HttpContext.Current.Server.GetLastError();
                    var request = HttpContext.Current.Request;
                    var correlationId = (new HttpUtils().GetCorrelationId(request));
                    LogUnhandledError(ex, correlationId);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
