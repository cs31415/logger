using System;
using Logger.Models;
using TestApi.Models;

namespace TestApi.Builders
{
    public interface IApplicationLogRecordBuilder
    {
        ApplicationLogRecord Build(int rnd, string message, string correlationId);
        ApplicationLogRecord Build(LogLevel level, string message, string correlationId);
        ApplicationLogRecord Build(Exception ex, string correlationId);
    }
}