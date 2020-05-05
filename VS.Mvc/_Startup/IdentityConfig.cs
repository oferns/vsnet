namespace VS.Mvc._Startup {
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using VS.Abstractions;
    using VS.Mvc._Services;

    public static class IdentityConfig {

        public static IServiceCollection AddAppIdentity(this IServiceCollection services) {
            services.AddAuthentication(
#if DEBUG                
                "DEV"
#endif                
                )
#if DEBUG                
                .AddCookie("DEV")
#endif
                .AddCookie();

            return services.AddAuthorization()
                .AddScoped<IContext, WebContext>();
        }


        public static IApplicationBuilder UseAppIdentity(this IApplicationBuilder app) {

            return app.UseAuthentication()
                    .UseAuthorization();
        }
    }
}
