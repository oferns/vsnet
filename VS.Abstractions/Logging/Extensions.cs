
namespace VS.Abstractions.Logging {
    using System;

    public static class LoggerExtensions {
        public static void Log(this ILog logger, string message, Exception exception = default) {
            logger.Log(new LogEntry(LogEventType.Information, message, exception));
        }

        public static void LogError(this ILog logger, Exception exception) {
            logger.Log(new LogEntry(LogEventType.Error, exception.Message, exception));
        }

        public static void LogInfo(this ILog logger, string message, Exception exception = default) {
            logger.Log(new LogEntry(LogEventType.Information, message, exception));
        }

        public static void LogDebug(this ILog logger, string message, Exception exception = default) {
            logger.Log(new LogEntry(LogEventType.Debug, message, exception));
        }


        public static void LogWarn(this ILog logger, string message, Exception exception = default) {
            logger.Log(new LogEntry(LogEventType.Warning, message, exception));
        }

    }

}