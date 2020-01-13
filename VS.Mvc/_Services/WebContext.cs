namespace VS.Mvc._Services {

    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security.Claims;
    using VS.Abstractions;

    public class WebContext : IContext {

        public WebContext(IHttpContextAccessor contextAccessor) {
            if (contextAccessor?.HttpContext is null) {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            this.User = (ClaimsPrincipal)contextAccessor.HttpContext.User; 
            this.Host = contextAccessor.HttpContext.Request.Host.Host;
            this.UICulture = CultureInfo.CurrentUICulture;
            this.RequestId = Activity.Current?.RootId ?? contextAccessor.HttpContext.TraceIdentifier;
        }

        public ClaimsPrincipal User { get; }

        public string Host { get; }

        public string RequestId { get; }

        public CultureInfo UICulture { get; }

    }
}