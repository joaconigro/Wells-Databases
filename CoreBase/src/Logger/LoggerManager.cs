using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Wells.Utilities.LoggerService
{
    public class LoggerManager : Logger, ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerManager() : base()
        {
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogError(Exception ex)
        {
            LogError(ex.Message);
            LogError(ex.StackTrace);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

        public void Log(string message, TraceEventType severity)
        {
            switch (severity)
            {
                case TraceEventType.Information:
                    LogInfo(message);
                    break;
                case TraceEventType.Warning:
                    LogWarn(message);
                    break;
                case TraceEventType.Error:
                case TraceEventType.Critical:
                    LogError(message);
                    break;
                default:
                    LogDebug(message);
                    break;
            }
        }
    }
}
