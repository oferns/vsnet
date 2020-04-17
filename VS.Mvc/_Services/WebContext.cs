namespace VS.Mvc._Services {

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security.Claims;
    using TimeZoneConverter;
    using VS.Abstractions;
    using VS.Abstractions.Logging;
    using VS.Core.Identity;

    public class WebContext : IContext {

            public WebContext(IHttpContextAccessor contextAccessor, ILog log) {
            if (contextAccessor is null) {
                throw new ArgumentNullException(nameof(contextAccessor));
            }
            
            Log = log ?? throw new ArgumentNullException(nameof(log));

            this.User = contextAccessor.HttpContext?.User;
            this.Host = contextAccessor.HttpContext?.Items["HostIdentity"] as ClaimsIdentity;
            this.UICulture = CultureInfo.CurrentUICulture;
            this.RequestId = Activity.Current?.RootId ?? contextAccessor.HttpContext?.TraceIdentifier;
            this.UserTimeZone = TZConvert.GetTimeZoneInfo("Europe/London");

        }

        public ClaimsPrincipal User { get; }

        public ClaimsIdentity Host { get; }

        public string RequestId { get; }

        public CultureInfo UICulture { get; }

        public ILog Log { get; }

        public TimeZoneInfo UserTimeZone { get; }
    }
}