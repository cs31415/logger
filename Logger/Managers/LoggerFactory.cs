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
        public static string Application { get; private set; }
        public static string ApplicationVersion { get; private set; }
        public static string  Environment { get; private set; }
        public static string  AccessLogName { get; private set; }
        public static ILoggerFactory Instance { get; private set; }

        private LoggerFactory(string [] loggerTypes)
        {
            _loggers = new Dictionary<string, ILog>();
            log4net.Config.XmlConfigurator.Configure();
            foreach (var loggerType in loggerTypes)
            {
                _loggers.Add(loggerType, LogManager.GetLogger(loggerType));
            }    
        }

        public static ILoggerFactory GetLoggerFactoryInstance(string [] loggerTypes, string application, string applicationVersion, string environment, string accessLogName)
        { 
            if (Instance == null)
            {
                Application = application;
                ApplicationVersion = applicationVersion;
                Environment = environment;
                AccessLogName = accessLogName;
                Instance = new LoggerFactory(loggerTypes);
            }

            return Instance;
        }
        
        public ILogger GetLogger(string type)
        {
            var logger = _loggers[type];
            return new Logger(logger);
        }
        
        public ILogRecordBuilder GetLogRecordBuilder()
        {
            return new LogRecordBuilder(Application, ApplicationVersion, Environment);
        }
        
        public IAccessLogRecordBuilder GetAccessLogRecordBuilder()
        {
            return new AccessLogRecordBuilder(Application, ApplicationVersion, Environment);
        }
    }
}
