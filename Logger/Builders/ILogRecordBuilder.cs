using System;
using Logger.Models;

namespace Logger.Builders
{
    public interface ILogRecordBuilder
    {
        LogRecord Build(LogLevel level, string message, string correlationId);
        LogRecord Build(Exception ex, string correlationId);
    }
}