namespace VS.Mvc._Services {
    using Serilog;
    using VS.Abstractions.Logging;

    public class SeriLogger : ILog {

        private readonly ILogger serilog;

        public SeriLogger(ILogger serilog) {
            this.serilog = serilog ?? throw new System.ArgumentNullException(nameof(serilog));
        }

        public void Log(LogEntry entry) {
            switch (entry.Severity) {
                case LogEventType.Debug:
                    serilog.Debug(entry.Exception, entry.Message);
                    break;
                case LogEventType.Information:
                    serilog.Information(entry.Exception, entry.Message);
                    break;
                case LogEventType.Warning:
                    serilog.Warning(entry.Exception, entry.Message);
                    break;
                case LogEventType.Error:
                    serilog.Error(entry.Exception, entry.Message);
                    break;
                case LogEventType.Fatal:
                    serilog.Fatal(entry.Exception, entry.Message);
                    break;
            }
        }
    }
}