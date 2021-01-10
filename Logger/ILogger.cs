namespace Logger
{
    public interface ILogger
    {
        void LogInfo(string msg);
        void LogInfo(object logRecord);
        void LogDebug(string msg);
        void LogDebug(object logRecord);
        void LogError(string msg);
        void LogError(object errorRecord);
    }
}