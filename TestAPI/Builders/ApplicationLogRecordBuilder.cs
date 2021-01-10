using System;
using Logger.Builders;
using Logger.Models;
using TestApi.Models;

namespace TestApi.Builders
{
    /// <summary>
    /// Builds application log record
    /// </summary>
    public class ApplicationLogRecordBuilder : IApplicationLogRecordBuilder
    {
        private readonly ILogRecordBuilder _logRecordBuilder;
        public ApplicationLogRecordBuilder(
            string application,
            string applicationVersion,
            string environment)
        {
            _logRecordBuilder = new LogRecordBuilder(application, applicationVersion, environment);
        }

        public ApplicationLogRecord Build(int rnd, string message, string correlationId)
        {
            var logRecord = _logRecordBuilder.Build(LogLevel.Info, message, correlationId);
            var applicationLogRecord = new ApplicationLogRecord(logRecord)
            {
                Rnd = rnd
            };
            return applicationLogRecord;
        }

        public ApplicationLogRecord Build(Exception ex, string correlationId)
        {
            var logRecord = _logRecordBuilder.Build(ex, correlationId);
            return new ApplicationLogRecord(logRecord);
        }

        public ApplicationLogRecord Build(LogLevel level, string message, string correlationId)
        {
            var logRecord = _logRecordBuilder.Build(level, message, correlationId);
            return new ApplicationLogRecord(logRecord);
        }
        
        
    }
}
