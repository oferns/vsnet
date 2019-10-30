namespace VS.Mvc {
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using VS.Mvc.Extensions;
    using VS.Mvc.Services;
    using VS.Mvc.StartupTasks;

    public class Startup {

        internal readonly IConfiguration configuration;
        internal readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public void ConfigureServices(IServiceCollection services) {
            
            services.AddMiniProfiler();
            
            services                
                .AddConstraints()
                .AddHttpContextAccessor()
                .AddLocalization()
                .AddAuthenticationCore()
                .AddAuthorizationCore()
                .AddViewOptions()
                // MVC Builder
                .AddControllersWithViews(o => o.Conventions.Add(new RouteTokenTransformerConvention(
                             new SlugifyParameterTransformer())))
                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder);
        }

        // OJF: ORDER IS IMPORTANT. ONLY CHANGE IF YOU KNOW WHAT YOU ARE DOING AND WHY AND IT BETTER BE IN THE COMMIT MESSAGE (and yes, I did mean to shout that).
        public void Configure(IApplicationBuilder app) {

            // I believe it is correctly configured but the analyzer can't cope with chained methods
#pragma warning disable ASP0001 // Authorization middleware is incorrectly configured.
            
            app.UseMiniProfiler()
                .ProxyForwardHeaders()
                .UseStaticFiles() 
                .UseRequestLocalization(o => {
                    o.SetDefaultCulture("en-GB");
                    o.SupportedCultures.Add(new System.Globalization.CultureInfo("en-GB"));
                    o.SupportedCultures.Add(new System.Globalization.CultureInfo("fr-FR"));
                    o.SupportedUICultures.Add(new System.Globalization.CultureInfo("en-GB"));
                    o.SupportedUICultures.Add(new System.Globalization.CultureInfo("fr-FR"));
                    o.RequestCultureProviders.Insert(0, new HostBasedRequestCultureProvider());
                })
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