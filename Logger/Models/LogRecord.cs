using System;

namespace Logger.Models
{
    /// <summary>
    /// Base log record
    /// </summary>
    public class LogRecord
    {
        public string CorrelationId { get; set; }
        public string Application { get; set; }
        public string ApplicationVersion { get; set; }
        public string Host { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string ErrorType { get; set; }
        public string StackTrace { get;set; }
        public string Environment { get; set; }

        // { .NET, node.js etc. }
        public string Runtime { get; set; }
        
    }
}