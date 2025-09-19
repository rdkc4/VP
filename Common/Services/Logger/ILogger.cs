using System;

namespace Common.Services.Logger
{
    public interface ILogger : IDisposable
    {
        void Log(string _event, string message);
        void LogEvent(string _event, string message);
        void LogLeftover(string _event, string message);
    }
}
