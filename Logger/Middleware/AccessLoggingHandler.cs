using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Logger;
using Logger.Helpers;
using Logger.Managers;

namespace logger.Middleware
{
    /// <summary>
    /// Delegating handler that writes a structured access log record
    /// </summary>
    public class AccessLoggingHandler : DelegatingHandler
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        
        public AccessLoggingHandler(ILoggerFactory loggerFactory, string accessLogName)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.GetLogger(accessLogName);
        }
        
        /// <summary>
        /// Generate access log record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Task<HttpResponseMessage> responseTask = base.SendAsync(request, cancellationToken);
            HttpResponseMessage response = await responseTask;
            timer.Stop();

            var correlationId = (new HttpUtils().GetCorrelationId(request));
            
            var responseTimeMs = timer.ElapsedMilliseconds;  
            
            var builder = _loggerFactory.GetAccessLogRecordBuilder();
            _logger.LogInfo(builder.Build(string.Empty, request, response, responseTimeMs, correlationId));

            return response;
        }
    }
}
