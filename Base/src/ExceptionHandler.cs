using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Wells.Utilities.LoggerService;

namespace Wells.Base
{
    public static class ExceptionHandler
    {

        private static LoggerManager loggerManager;

        public static LoggerManager LoggerManager
        {
            get
            {
                if (loggerManager == null)
                {
                    LogManager.LoadConfiguration(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
                    loggerManager = new LoggerManager();
                }
                return loggerManager;
            }
        }

        public static void Handle(Exception ex)
        {
            Handle(ex, TraceEventType.Error, "", true);
        }

        public static void Handle(Exception ex, bool rethrow)
        {
            Handle(ex, TraceEventType.Error, "", rethrow);
        }

        public static void Handle(Exception ex, TraceEventType severity, string additionalInfo)
        {
            Handle(ex, TraceEventType.Error, "", true);
        }

        public static void Handle(Exception ex, TraceEventType severity, string additionalInfo, bool rethrow)
        {
            if (ex != null)
            {
                LoggerManager.Log($"Exception type {ex.GetType().Name}", severity);
                Log(ex, severity, additionalInfo);
                var inner = ex.InnerException;
                while (inner != null)
                {
                    LoggerManager.Log($"Exception type {ex.InnerException.GetType().Name}", severity);
                    Log(ex.InnerException, severity, additionalInfo);
                    inner = inner.InnerException;
                }
                if (rethrow) { throw ex; }
            }
        }



        private static void Log(Exception ex, TraceEventType severity, string additionalInfo)
        {
            LoggerManager.Log("", severity);
            LoggerManager.Log(additionalInfo, severity);
            LoggerManager.Log(ex.StackTrace, severity);
        }

        public static string GetString(Exception ex)
        {
            var msg = new StringBuilder();
            msg.AppendLine(ex.Message);
            msg.AppendLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                msg.AppendLine(ex.InnerException.Message);
                msg.AppendLine(ex.InnerException.StackTrace);
            }
            return msg.ToString();
        }


        public static string GetAllMessages(Exception ex)
        {
            var msg = new StringBuilder();
            msg.AppendLine(ex.Message);
            var inner = ex.InnerException;
            while (inner != null)
            {
                msg.AppendLine(inner.Message);
                inner = inner.InnerException;
            }

            return msg.ToString();
        }
    }
}
