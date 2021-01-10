using System;
using System.Net;
using System.Reflection;
using System.Web;
using Logger.Models;

namespace Logger.Builders
{
    /// <summary>
    /// Builds base log record
    /// </summary>
    public class LogRecordBuilder : ILogRecordBuilder
    {
        private readonly string _application;
        private readonly string _applicationVersion;
        private readonly string _environment;
        private static string _runTimeVersion;
        
        public LogRecordBuilder(
            string application, 
            string applicationVersion, 
            string environment)
        {
            _application = application;
            _applicationVersion = applicationVersion;
            _environment = environment;
        }

        public LogRecord Build(LogLevel level, string message, string correlationId)
        {
            return new LogRecord 
            { 
                CorrelationId = correlationId,
                Application = _application,
                ApplicationVersion = _applicationVersion,
                Environment = _environment,
                Runtime = GetRuntimeVersion(),
                Level = level.ToString(),
                Message = message ?? string.Empty,
                ErrorType = string.Empty,
                StackTrace = string.Empty,
                Host = Dns.GetHostName(),
                TimeStamp = DateTime.UtcNow
            };
        }

        public LogRecord Build(Exception ex, string correlationId)
        {
            return new LogRecord 
            { 
                CorrelationId = correlationId,
                Application = _application,
                ApplicationVersion = _applicationVersion,
                Environment = _environment,
                Runtime = GetRuntimeVersion(),
                Level = LogLevel.Error.ToString(),
                Message = ex.Message,
                ErrorType = ex.GetType().Name,
                StackTrace = ex.StackTrace,
                Host = Dns.GetHostName(),
                TimeStamp = DateTime.UtcNow
            };
        }
        
        private string GetRuntimeVersion()
        {
            if (_runTimeVersion == null)
            {
                var netVer = Environment.Version.ToString();
                var assObj = typeof(Object).GetTypeInfo().Assembly;
                var attr = (AssemblyFileVersionAttribute) assObj.GetCustomAttribute(
                    typeof(AssemblyFileVersionAttribute));
                //_runTimeVersion = $".NET {attr?.Version ?? netVer}";
                _runTimeVersion = $".NET {HttpRuntime.TargetFramework}";
            }

            return _runTimeVersion;
        }
    }
}
