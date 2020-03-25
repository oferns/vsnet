namespace VS.Mvc {
    using System;
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using SimpleInjector;
    using SimpleInjector.Lifestyles;
    using VS.Mvc._Extensions;
    using VS.Mvc._Services;
    using VS.Mvc._Startup;

    public class Startup {
        internal readonly Container container;
        internal readonly IConfiguration configuration;
        internal readonly IWebHostEnvironment env;
        internal IServiceCollection services;

        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.env = env ?? throw new ArgumentNullException(nameof(env));
            this.container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        }

        public void ConfigureServices(IServiceCollection services) {
            
            var cultureoptions = configuration.GetSection("CultureOptions").Get<CultureOptions>();
            
            this.services = services
                    .AddLog()
                    .AddSingleton<CultureOptions>(cultureoptions)
#if DEBUG
                    .AddDevServices()
#endif                    
                    .AddRequestCorrelation()
                    .AddConstraints()
                    .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                    .AddHttpContextAccessor()
                    .AddHttpClient()
                    .AddHostBasedLocalization(cultureoptions.HostOptions)
                    .AddAppIdentity()
                    .AddViewOptions(configuration.GetSection("AntiforgeryOptions").Get<AntiforgeryOptions>())
                    .AddSimpleInjector(container, options => {
                        options
                           .AddAspNetCore()
                           .AddControllerActivation()
                           .AddViewComponentActivation()
                           .AddTagHelperActivation();

                        container                            
                            .AddAwsServices(configuration, Log.Logger)
                            .AddPayOn(configuration, Log.Logger)
                            .AddCoreServices()
                            .AddCaching(configuration, Log.Logger)
                            .AddPostGresServices(configuration, Log.Logger)
                            .AddSerializationServices();
                    });
        }

        // OJF: ORDER IS IMPORTANT. ONLY CHANGE IF YOU KNOW WHAT YOU ARE DOING AND WHY AND IT BETTER BE IN THE COMMIT MESSAGE (and yes, I did mean to shout that).
        public void Configure(IApplicationBuilder app) {

            // I believe it is correctly configured but the analyzer can't cope with chained methods
            // Remove the pragma clause and make your own mind up.
#pragma warning disable     

            _ = app
                .UseSerilogRequestLogging()
                .ProxyForwardHeaders()
                .UseStaticFiles(new StaticFileOptions() { ContentTypeProvider = new WebManifestTypeContentProvider() })
                .UseRequestCorrelation()
                .UseSimpleInjector(container)
                .UseHostBasedLocalization()
                .UseExceptionHandler("/error")
                .UseStatusCodePagesWithReExecute("/error", "?sc={0}")
                .UseRouting()
                .UseAppIdentity()
#if DEBUG
                .UseDevTools()
#endif    
                .UseEndpoints(e => e.AddMvcEndpoints());

#pragma warning restore ASP0001 // Authorization middleware is incorrectly configured.


            container.Verify();
        }
    }
}