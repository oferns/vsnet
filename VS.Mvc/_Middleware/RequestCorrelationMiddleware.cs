namespace VS.Mvc._Middleware {
  
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class RequestCorrelationMiddleware : IMiddleware {

        private const string HEADER_NAME = "vscl";

        public Task InvokeAsync(HttpContext context, RequestDelegate next) {

            if (context.Request.Cookies.TryGetValue(HEADER_NAME, out string correlationId)) {
                context.TraceIdentifier = correlationId;
                var id = Activity.Current?.Id;

            }

            // apply the correlation ID to the response header for client side tracking
            context.Response.OnStarting(() => {
               // context.Response.Headers.Add(HEADER_NAME, new[] { context.TraceIdentifier });
                context.Response.Cookies.Append(HEADER_NAME, context.TraceIdentifier);
                return Task.CompletedTask;
            });

            return next(context);
        }
    }
}
