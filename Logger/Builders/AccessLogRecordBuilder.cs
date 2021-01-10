using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Logger.Models;

namespace Logger.Builders
{
    /// <summary>
    /// Builds access log record
    /// </summary>
    public class AccessLogRecordBuilder : IAccessLogRecordBuilder
    {
        private readonly ILogRecordBuilder _logRecordBuilder;
        public AccessLogRecordBuilder(
            string application, 
            string applicationVersion, 
            string environment)
        {
            _logRecordBuilder = new LogRecordBuilder(
                application, applicationVersion, environment);
        }

        public AccessLogRecord Build(string message, HttpRequestMessage request, HttpResponseMessage response, long responseTimeMs, string correlationId)
        {
            var logRecord = _logRecordBuilder.Build(LogLevel.Info, message, correlationId);
            var accessLogRecord = new AccessLogRecord(logRecord)
            {
                Url = request.RequestUri.AbsoluteUri,
                StatusCode = response.StatusCode.ToString(),
                Method = request.Method.Method,
                RequestHeaders = ReduceHeaders(request.Headers),
                ResponseHeaders = ReduceHeaders(response.Headers),
                ResponseTimeMs = responseTimeMs
            };
            return accessLogRecord;
        }

        private string ReduceHeaders(HttpHeaders headers)
        {
            if (headers?.Count() == 0)
            {
                return string.Empty;
            }
            // Filter any headers here if required
            return Aggregate(headers?.Select(h => $"{h.Key}:{Aggregate(h.Value)}"));
        }

        private string Aggregate(IEnumerable<string> values)
        {
            return values.Aggregate((x, y) => $"{x},{y}");
        }
    }
}
