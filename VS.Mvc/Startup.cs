namespace VS.Mvc {


    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using VS.Mvc.Extensions;
    using VS.Mvc.StartupTasks;

    public class Startup {

        internal readonly IConfiguration configuration;
        internal readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            this.env = env ?? throw new System.ArgumentNullException(nameof(env));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {

            services.AddRouting(o => {
                o.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });

            services.AddLocalization();

            services.AddAuthenticationCore();
            services.AddAuthorizationCore();

            services.AddMvc();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ORDER IS IMPORTANT. ONLY CHANGE IF YOU KNOW WHAT YOU ARE DOING AND WHY AND IT BETTER BE IN THE COMMIT MESSAGE.
        public void Configure(IApplicationBuilder app) {
            app.ForwardHeaders()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(e => e.AddMvc())
                .UseExceptionHandler("/error")
                .UseStatusCodePagesWithReExecute("/error", "?sc={0}"); 

            
        }
    }
}