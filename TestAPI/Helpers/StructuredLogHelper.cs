using System;
using Logger;
using Logger.Managers;
using Logger.Models;
using TestApi.Builders;

namespace TestApi.Helpers
{
    /// <summary>
    /// Helper class that creates a structured JSON log record
    /// </summary>
    public class StructuredLogHelper : IStructuredLogHelper
    {
        private readonly ILogger _logger;
        private readonly IApplicationLogRecordBuilder _logRecordBuilder;

        public StructuredLogHelper(ILoggerFactory loggerFactory, IApplicationLogRecordBuilder logRecordBuilder)
        {
            _logger = loggerFactory.GetLogger(LoggerTypes.Application.ToString());
            _logRecordBuilder = logRecordBuilder;
        }

        public void LogInfo(object logRecord)
        {
            _logger.LogInfo(logRecord);
        }

        public void LogInfo(string message, string correlationId)
        {
            var logRecord = _logRecordBuilder.Build(LogLevel.Info, message, correlationId);
            _logger.LogInfo(logRecord);
        }

        public void LogInfo(int rnd, string message, string correlationId)
        { 
            var logRecord = _logRecordBuilder.Build(rnd, message, correlationId);
            _logger.LogInfo(logRecord);
        }
        
        public void LogDebug(object logRecord)
        {
            _logger.LogDebug(logRecord);
        }

        public void LogDebug(string message, string correlationId)
        {
            var logRecord = _logRecordBuilder.Build(LogLevel.Debug, message, correlationId);
            _logger.LogInfo(logRecord);
        }

        public void LogError(Exception ex, string correlationId)
        {
            var logRecord = _logRecordBuilder.Build(ex, correlationId);
            _logger.LogError(logRecord);
        }

        public void LogError(string message, string correlationId)
        {
            var logRecord = _logRecordBuilder.Build(LogLevel.Error, message, correlationId);
            _logger.LogError(logRecord);
        }
    }
}
