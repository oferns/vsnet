namespace VS.Mvc._Middleware {
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Abstractions.Logging;
    using VS.Core.Analytics;

    public class AnalyticsMiddleware : IMiddleware {


        private static readonly byte[] transparentPng = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7");
        private readonly IProducerConsumerCollection<UserEvent> collection;        

        public AnalyticsMiddleware(IProducerConsumerCollection<UserEvent> collection) {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));            
        }


        public Task InvokeAsync(HttpContext context, RequestDelegate next) {
            if (context is null) {
                throw new ArgumentNullException(nameof(context));
            }
            Dns.GetHostName();
            if (next is null) {
                throw new ArgumentNullException(nameof(next));
            }

            var events = new List<string>();
            foreach (var cookie in context.Request.Cookies) {
                if (cookie.Key.StartsWith("vsa", StringComparison.Ordinal)) {
                    events.Add(WebUtility.UrlDecode(cookie.Value));
                    context.Response.Cookies.Append(cookie.Key, string.Empty, new CookieOptions { Expires = DateTimeOffset.Now + TimeSpan.FromDays(-1) });
                }
            }

            if (events.Count > 0) {
                var ev = new UserEvent { Events = events };
                
                if (!collection.TryAdd(ev)) {
                  //  appContext.Log.LogWarn("Event could not be added to the consumeer queue and will not be recorded");   
                }
            }

            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.Headers.Add(HeaderNames.ContentType, "image/png");
            context.Response.Headers.Add(HeaderNames.ContentLength, $"{transparentPng.Length}");
            context.Response.Headers.Add(HeaderNames.CacheControl, "private, no-cache, no-cache=Set-Cookie, proxy-revalidate");
            context.Response.Headers.Add(HeaderNames.Expires, "Wed, 11 Jan 2000 12:59:00 UTC");
            context.Response.Headers.Add(HeaderNames.LastModified, "Wed, 11 Jan 2006 12:59:00 UTC");
            context.Response.Headers.Add(HeaderNames.Pragma, "no-cache");

            return context.Response.Body.WriteAsync(transparentPng).AsTask();            
        }
    }
}
