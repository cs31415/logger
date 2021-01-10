namespace Logger.Models
{
    /// <summary>
    /// Access log record for HTTP requests
    /// </summary>
    public class AccessLogRecord : LogRecord
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string StatusCode { get; set; }
        public string RequestHeaders { get; set; }
        public string ResponseHeaders { get; set; }
        public long ResponseTimeMs { get; set; }

        public AccessLogRecord(LogRecord logRecord)
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
