using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Logger.Managers;

namespace Logger.Middleware
{
    public class ExceptionLoggingHandler : DelegatingHandler
    {
        private readonly Action<string, string> _logUnhandledException;

        public ExceptionLoggingHandler(Action<string, string> logUnhandledException)
        {
            _logUnhandledException = logUnhandledException;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var correlationId = request.Headers.GetValues("X-CorrelationId").FirstOrDefault();
                _logUnhandledException(content, correlationId);
            }

            return response;
        }
    }
}
