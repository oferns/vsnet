namespace VS.Mvc._Middleware.DevOnly {

    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VS.Mvc._Extensions;

    public class HostSwitchingMiddleware : IMiddleware {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            if (context.Request.Query["host"] is object host) {
                context.Response.Cookies.Append("vshost", host.ToString());
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
