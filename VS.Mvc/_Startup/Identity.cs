﻿namespace VS.Mvc._Startup {
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using VS.Abstractions;
    using VS.Mvc._Services;

    public static class Identity {

        public static IServiceCollection AddAppIdentity(this IServiceCollection services) {
            return services
                .AddAuthenticationCore()
                .AddAuthorizationCore()
                .AddScoped<IContext, WebContext>();
        }


        public static IApplicationBuilder UseAppIdentity(this IApplicationBuilder app) {

            return app.UseAuthentication()
                    .UseAuthorization();
        }
    }
}
