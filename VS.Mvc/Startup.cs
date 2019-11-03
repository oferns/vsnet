namespace VS.Mvc {
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using VS.Mvc._Extensions;
    using VS.Mvc._Services;
    using VS.Mvc._Startup;


    public class Startup {

        internal readonly IConfiguration configuration;
        internal readonly IWebHostEnvironment env;
        internal IServiceCollection services;

        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public void ConfigureServices(IServiceCollection services) {

            this.services = services
                    .AddSingleton<HostCultureOptions[]>(configuration.GetSection("HostCultureOptions").Get<HostCultureOptions[]>())
#if DEBUG
                    .AddMiniProfiler().Services
#endif                    
                    .AddConstraints()  
                    .AddHttpContextAccessor()
                    .AddHostBasedLocalization()
                    .AddAuthenticationCore()
                    .AddAuthorizationCore()
                    .AddViewOptions()
                    // MVC Builder
                    .AddControllersWithViews(o => o.Conventions.Add(new RouteTokenTransformerConvention(
                                 new SlugifyParameterTransformer())))
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder).Services;
        }

        // OJF: ORDER IS IMPORTANT. ONLY CHANGE IF YOU KNOW WHAT YOU ARE DOING AND WHY AND IT BETTER BE IN THE COMMIT MESSAGE (and yes, I did mean to shout that).
        public void Configure(IApplicationBuilder app) {

            // I believe it is correctly configured but the analyzer can't cope with chained methods
            // Remove the pragma clause and make your own mind up.
#pragma warning disable ASP0001 // Authorization middleware is incorrectly configured.

            _ = app
#if DEBUG
                .UseMiniProfiler()
#endif
                .ProxyForwardHeaders()
                .UseStaticFiles()
                .UseHostBasedLocalization()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(e => e.AddMvcEndpoints())
                .UseExceptionHandler("/error")
                .UseStatusCodePagesWithReExecute("/error", "?sc={0}");

#pragma warning restore ASP0001 // Authorization middleware is incorrectly configured.

        }
    }
}