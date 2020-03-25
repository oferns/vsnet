namespace VS.Mvc._Startup {
    using System.Collections.Generic;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Localization;
    using VS.Core.Identity;
    using VS.Mvc._Extensions;
    using VS.Mvc._Middleware;
    using VS.Mvc._Services;

    public static class HostBasedLocalization {


        public static IServiceCollection AddHostBasedLocalization(this IServiceCollection services, IEnumerable<HostCultureOptions> hostOptions) {



            return services
                    .AddSingleton<ClaimsIdentity[]>((c) => {
                        var list = new List<ClaimsIdentity>();
                        foreach (var host in hostOptions) {
                            list.Add(new ClaimsIdentity(new[] {
                                new Claim(IdClaimTypes.HostIdentifier, host.Host),
                                new Claim(IdClaimTypes.Timezone, host.DefaultTimezone)
                            }));
                        }

                        return list.ToArray();
                    })
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
