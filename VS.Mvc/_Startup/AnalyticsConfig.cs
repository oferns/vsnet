
namespace VS.Mvc._Startup {

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Concurrent;
    using VS.Core.Analytics;
    using VS.Mvc._Middleware;
    using VS.Mvc._Services;

    public static class AnalyticsConfig {

        public static IServiceCollection AddAnalytics(this IServiceCollection services) {
            return services.AddSingleton<AnalyticsMiddleware>()
                .AddSingleton<IProducerConsumerCollection<UserEvent>, ConcurrentQueue<UserEvent>>()
                .AddSingleton<AnalyticsHostedService>()
                .AddHostedService<AnalyticsHostedService>(s => s.GetService<AnalyticsHostedService>())
            //.AddHostedService<AnalyticsHostedService>()
            ;
        }

        public static IApplicationBuilder UseAnalytics(this IApplicationBuilder app) {
            return app.Map("/an", b => b.UseMiddleware<AnalyticsMiddleware>());
        }
    }
}
