namespace VS.Abstractions.Logging {

    using System;

    public class LogEntry {

        public LogEntry(LogEventType severity, string message, Exception exception = default) {
            if (string.IsNullOrEmpty(message)) {
                throw new ArgumentNullException(nameof(message));
            }

            Severity = severity;
            Message = message;
            Exception = exception;
        }

        public LogEventType Severity { get; }
        public string Message { get; }
        public Exception Exception { get; }
    }
}