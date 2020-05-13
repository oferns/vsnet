namespace VS.Mvc._Middleware {
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class RequestCorrelationMiddleware : IMiddleware {

        private const string HEADER_NAME = "vscl";

        public Task InvokeAsync(HttpContext context, RequestDelegate next) {
            if (context is null) {
                throw new ArgumentNullException(nameof(context));
            }

            if (next is null) {
                throw new ArgumentNullException(nameof(next));
            }

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
