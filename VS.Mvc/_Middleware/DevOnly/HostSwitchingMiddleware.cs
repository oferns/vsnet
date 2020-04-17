namespace VS.Mvc._Middleware.DevOnly {

    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;
    using System;
    using System.Threading.Tasks;
    using VS.Abstractions.Culture;
    using VS.Mvc._Extensions;

    public class HostSwitchingMiddleware : IMiddleware {
        private readonly CultureOptions options;

        public HostSwitchingMiddleware(CultureOptions options) {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            if (context.Request.Query["host"] is object host) {

                bool validhost = false;
                foreach (var opt in this.options.HostOptions) {
                    if (opt.Host.Equals(host.ToString(), StringComparison.InvariantCultureIgnoreCase)) {
                        validhost = true;
                    }
                }
                
                if (validhost) {
                    context.Response.Cookies.Append("vshost", host.ToString());
                } else {
                    context.Response.Cookies.Delete("vshost");
                }
                var referer = context.Request.Headers[HeaderNames.Referer].ToString();
                var backto = context.IsLocalUrl(referer) ? referer : "/";
                backto = backto.Equals(context.Request.Path) ? "/" : backto;
                context.Response.Redirect(backto);

            } else {
                await next(context);
            }
        }
    }
}
