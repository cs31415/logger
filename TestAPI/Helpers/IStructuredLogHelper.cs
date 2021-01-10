using System;

namespace TestApi.Helpers
{
    public interface IStructuredLogHelper
    {
        void LogInfo(int rnd, string message, string correlationId);
        void LogError(Exception ex, string correlationId);
        void LogInfo(object logRecord);
        void LogInfo(string message, string correlationId);
        void LogDebug(object logRecord);
        void LogDebug(string message, string correlationId);
    }
}