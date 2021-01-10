using Logger.Builders;

namespace Logger.Managers
{
    public interface ILoggerFactory
    {
        ILogger GetLogger(string type);

        ILogRecordBuilder GetLogRecordBuilder();

        IAccessLogRecordBuilder GetAccessLogRecordBuilder();
    }
}