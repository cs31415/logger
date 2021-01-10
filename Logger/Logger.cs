using log4net;
using Newtonsoft.Json;

namespace Logger
{
    /// <summary>
    /// Logger facade (wrapper around log4net for logging objects as json)
    /// </summary>
    public class Logger : ILogger
    {
        private readonly ILog _log;

        public Logger(ILog log)
        {
            _log = log;
        }

        public void LogInfo(string msg)
        {
            _log.Info(msg);
        }
        
        public void LogInfo(object logRecord)
        {
            _log.Info(JsonConvert.SerializeObject(logRecord));
        }
        
        public void LogDebug(string msg)
        {
            _log.Debug(msg);
        }
        
        public void LogDebug(object logRecord)
        {
            _log.Debug(JsonConvert.SerializeObject(logRecord));
        }
        
        public void LogError(string msg)
        {
            _log.Error(msg);
        }
        
        public void LogError(object errorRecord)
        {
            _log.Error(JsonConvert.SerializeObject(errorRecord));
        }
    }
}
