namespace VS.Mvc.StartupTasks {

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.HttpOverrides;

    /// <summary>
    /// Any actions expected from the reverse proxy
    /// </summary>
    public static class ReverseProxy {
        
        // <summary>
        /// Add headers forwarded by the web proxy, if there is one (ie nginx)
        /// </summary>
        public static IApplicationBuilder ForwardHeaders(this IApplicationBuilder app) {
            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            return app; 
        }
    }
}
