namespace VS.Mvc._Startup {
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using SimpleInjector;
    using VS.Abstractions.Logging;
    using VS.Core;
    using VS.Mvc._Services;

    public static class LogConfig {

        public static IServiceCollection AddLog(this IServiceCollection services) {

            services.AddScoped<ILog, SeriLogger>();
            return services;
        }


    }


}
