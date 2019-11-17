namespace VS.Mvc._Startup {

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Localization;
    using VS.Mvc._Extensions;
    using VS.Mvc._Middleware;
    using VS.Mvc._Services;

    public static class HostBasedLocalization {


        public static IServiceCollection AddHostBasedLocalization(this IServiceCollection services) {
            return services
                    .AddSingleton<HostBasedLocalizationMiddleware>()
                    .AddSingleton<LanguageSwitchingMiddleware>()
                    .AddSingleton<IRequestCultureProvider, CookieRequestCultureProvider>()
                    .AddSingleton<IRequestCultureProvider, AcceptLanguageHeaderRequestCultureProvider>()
                    .AddScoped<LocalizedRouteValueTransformer>()                    
                    .AddScoped<IStringLocalizer, StringLocalizer>();
        }


        public static IApplicationBuilder UseHostBasedLocalization(this IApplicationBuilder app) {

            return app
                    .UseMiddleware<HostBasedLocalizationMiddleware>()
                    .Map("/cl", b => b.UseMiddleware<LanguageSwitchingMiddleware>());
        }

    }
}
