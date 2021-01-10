using System.Net.Http;
using Logger.Models;

namespace Logger.Builders
{
    public interface IAccessLogRecordBuilder
    {
        AccessLogRecord Build(string message, HttpRequestMessage request, HttpResponseMessage response, long responseTimeMs, string correlationId);
    }
}