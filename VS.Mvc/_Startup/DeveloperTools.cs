namespace VS.Mvc._Startup {

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class DeveloperTools {

        public static IServiceCollection AddDevServices(this IServiceCollection services) {
            return services.AddMiniProfiler().Services;
        }

        public static IApplicationBuilder UseDevTools(this IApplicationBuilder app) {
            //var options = new StaticFileOptions
            //app.AddSpaStaticFiles(o => {
            //    o.Options.SourcePath = "wwwroot";                
            //    o.UseProxyToSpaDevelopmentServer("http://localhost:8083");
            //});
            app.UseBrowserLink();
            return app.UseMiniProfiler();
        }
    }
}
