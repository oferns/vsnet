namespace VS.Mvc._Startup {
    using Serilog;
    using SimpleInjector;
    using VS.Abstractions.Logging;
    using VS.Mvc._Services;

    public static class Logging {

        public static Container AddLogging(this Container container) {
            container.Register<ILogger>(() => Log.Logger);
            container.Register<ILog, SeriLogger>();
            return container;
        }
    }
}
