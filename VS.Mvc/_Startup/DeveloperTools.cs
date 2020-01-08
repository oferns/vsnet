﻿namespace VS.Mvc._Startup {

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System.Data;
    using VS.Mvc._Middleware.DevOnly;
    using VS.Mvc.Services.DevOnly;

    public static class DeveloperTools {

        public static IServiceCollection AddDevServices(this IServiceCollection services) {
            return services
                   .AddMiniProfiler().Services
                   .AddSingleton<HostSwitchingMiddleware>()
                   .AddSingleton<RouteGraphMiddleware>()
                   .AddScoped<IDbConnection, NoOpConnection>();
        }

        public static IApplicationBuilder UseDevTools(this IApplicationBuilder app) {

            return app
                    .UseMiniProfiler()
                    .Map("/cc", b => b.UseMiddleware<HostSwitchingMiddleware>())
                    .Map("/graph", b => b.UseMiddleware<RouteGraphMiddleware>())
                    .UseBrowserLink();
        }
    }
}
