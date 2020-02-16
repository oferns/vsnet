namespace VS.Mvc._Services {

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security.Claims;
    using VS.Abstractions;

    public class WebContext : IContext {

        public WebContext(IHttpContextAccessor contextAccessor) {
            if (contextAccessor is null) {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            this.User = contextAccessor.HttpContext?.User as ClaimsPrincipal; 
            this.Host = contextAccessor.HttpContext?.Request.Host.Host;
            this.Query = contextAccessor.HttpContext?.Request.Query;

#if DEBUG
            var hostoverride = this.User?.FindFirstValue("hostoverride");
            if (!string.IsNullOrEmpty(hostoverride)) {
                this.Host = hostoverride;
            }
#endif
            this.UICulture = CultureInfo.CurrentUICulture;
            this.RequestId = Activity.Current?.RootId ?? contextAccessor.HttpContext?.TraceIdentifier;
        }

        public ClaimsPrincipal User { get; }

        public string Host { get; }

        public string RequestId { get; }

        public CultureInfo UICulture { get; }

        public IEnumerable<KeyValuePair<string, StringValues>> Query { get; }
    }
}