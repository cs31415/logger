using System.Collections.Generic;
using log4net;
using Logger.Builders;

namespace Logger.Managers
{
    /// <summary>
    /// Factory for loggers and log record builders
    /// </summary>
    public class LoggerFactory : ILoggerFactory
    {
        readonly Dictionary<string, ILog> _loggers;
        private readonly string _application;
        private readonly string _applicationVersion;
        private readonly string _environment;

        public LoggerFactory(string [] loggerTypes, string application, string applicationVersion, string environment)
        {
            _application = application;
            _applicationVersion = applicationVersion;
            _environment = environment;
            _loggers = new Dictionary<string, ILog>();
            log4net.Config.XmlConfigurator.Configure();
            foreach (var loggerType in loggerTypes)
            {
                _loggers.Add(loggerType, LogManager.GetLogger(loggerType));
            }    
        }

        public ILogger GetLogger(string type)
        {
            var logger = _loggers[type];
            return new Logger(logger);
        }
        
        public ILogRecordBuilder GetLogRecordBuilder()
        {
            return new LogRecordBuilder(_application, _applicationVersion, _environment);
        }
        
        public IAccessLogRecordBuilder GetAccessLogRecordBuilder()
        {
            return new AccessLogRecordBuilder(_application, _applicationVersion, _environment);
        }
    }
}
