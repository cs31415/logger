using System.Net.Http;
using System.Web;
using Logger.Models;

namespace Logger.Builders
{
    public interface IAccessLogRecordBuilder
    {
        AccessLogRecord Build(string message, HttpRequestMessage request, HttpResponseMessage response, long responseTimeMs, string correlationId);

        AccessLogRecord Build(string message, HttpRequest request, HttpResponse response, long responseTimeMs,
            string correlationId);
    }
}