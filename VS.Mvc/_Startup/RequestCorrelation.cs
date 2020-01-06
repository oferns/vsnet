namespace VS.Mvc._Startup {

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VS.Mvc._Middleware;

    public static class RequestCorrelation {

        public static IServiceCollection AddRequestCorrelation(this IServiceCollection services) {
            return services.AddScoped<RequestCorrelationMiddleware>();
        }
        public static IApplicationBuilder UseRequestCorrelation(this IApplicationBuilder app) {
            return app.UseMiddleware<RequestCorrelationMiddleware>();
        }
    }
}