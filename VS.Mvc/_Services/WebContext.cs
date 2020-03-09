namespace VS.Mvc._Services {

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security.Claims;
    using VS.Abstractions;
    using VS.Abstractions.Logging;

    public class WebContext : IContext {

        public WebContext(IHttpContextAccessor contextAccessor, ILog log) {
            if (contextAccessor is null) {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            this.User = contextAccessor.HttpContext?.User as ClaimsPrincipal; 
            this.Host = contextAccessor.HttpContext?.Request.Host.Host;

#if DEBUG
            var hostoverride = this.User?.FindFirstValue("hostoverride");
            if (!string.IsNullOrEmpty(hostoverride)) {
                this.Host = hostoverride;
            }
#endif
            this.UICulture = CultureInfo.CurrentUICulture;
            this.RequestId = Activity.Current?.RootId ?? contextAccessor.HttpContext?.TraceIdentifier;
            Log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public ClaimsPrincipal User { get; }

        public string Host { get; }

        public string RequestId { get; }

        public CultureInfo UICulture { get; }
        
        public ILog Log { get; }
    }
}