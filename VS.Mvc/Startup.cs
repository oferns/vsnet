namespace VS.Mvc {
    using System;
    using Amazon.S3;
    using MediatR;
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using SimpleInjector;
    using VS.ActiveMQ;
    using VS.Aws;
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
        }

        public void ConfigureServices(IServiceCollection services) {

            this.services = services
                    .AddMediatR(this.GetType().Assembly)
                    .AddSingleton<CultureOptions>(configuration.GetSection("CultureOptions").Get<CultureOptions>())
#if DEBUG
                    .AddDevServices()
#endif                    
                    .AddRequestCorrelation()
                    .AddActiveMQServices(configuration.GetSection("MQueueOptions").Get<MQueueOptions>() ?? new MQueueOptions())
                    .AddConstraints()
                    .AddHttpContextAccessor()
                    .AddHostBasedLocalization()
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
                            .AddCoreServices();


                    });
        }

        // OJF: ORDER IS IMPORTANT. ONLY CHANGE IF YOU KNOW WHAT YOU ARE DOING AND WHY AND IT BETTER BE IN THE COMMIT MESSAGE (and yes, I did mean to shout that).
        public void Configure(IApplicationBuilder app) {

            // I believe it is correctly configured but the analyzer can't cope with chained methods
            // Remove the pragma clause and make your own mind up.
#pragma warning disable ASP0001 // Authorization middleware is incorrectly configured.

            _ = app
                .UseSerilogRequestLogging()
#if DEBUG
                .UseDevTools()
#endif             
                .ProxyForwardHeaders()
                .UseStaticFiles()
                .UseRequestCorrelation()
                .UseHostBasedLocalization()
                .UseExceptionHandler("/error")
                .UseStatusCodePagesWithReExecute("/error", "?sc={0}")
                .UseRouting()
                .UseAppIdentity()
                .UseEndpoints(e => e.AddMvcEndpoints());

#pragma warning restore ASP0001 // Authorization middleware is incorrectly configured.

        }
    }
}