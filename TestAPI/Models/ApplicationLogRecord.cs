using Logger.Models;

namespace TestApi.Models
{
    /// <summary>
    /// Application log record
    /// </summary>
    public class ApplicationLogRecord : LogRecord
    {
        public int Rnd { get; set; }
        
        // Add other application data here (for indexing and querying)
        
        
        public ApplicationLogRecord(LogRecord logRecord)
        {
            this.Application = logRecord.Application;
            this.ApplicationVersion = logRecord.ApplicationVersion;
            this.CorrelationId = logRecord.CorrelationId;
            this.Environment = logRecord.Environment;
            this.Host = logRecord.Host;
            this.Level = logRecord.Level;
            this.Message = logRecord.Message;
            this.ErrorType = logRecord.ErrorType;
            this.StackTrace = logRecord.StackTrace;
            this.TimeStamp = logRecord.TimeStamp;
            this.Runtime = logRecord.Runtime;
        }
    }
}
