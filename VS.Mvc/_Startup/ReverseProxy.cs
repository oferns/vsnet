namespace VS.Mvc._Startup {

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.HttpOverrides;

    /// <summary>
    /// Any actions expected from the reverse proxy
    /// </summary>
    public static class ReverseProxy {
        
        // <summary>
        /// Add headers forwarded by the web proxy, if there is one (ie nginx)
        /// </summary>
        public static IApplicationBuilder ProxyForwardHeaders(this IApplicationBuilder app) {
            var options = new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto                
            };

            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();

            return app.UseForwardedHeaders(options);             
        }
    }
}