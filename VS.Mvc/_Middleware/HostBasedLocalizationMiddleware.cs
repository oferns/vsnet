namespace VS.Mvc._Middleware {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Net.Http.Headers;
    using VS.Core.Identity;
    using VS.Abstractions.Culture;

    public class HostBasedLocalizationMiddleware : IMiddleware {

        private readonly CultureOptions options;
        private readonly IEnumerable<IRequestCultureProvider> providers;
        private readonly IEnumerable<ClaimsIdentity> hostIdentities;

        public HostBasedLocalizationMiddleware(CultureOptions options, IEnumerable<IRequestCultureProvider> providers, ClaimsIdentity[] hostIdentities) {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.providers = providers ?? throw new ArgumentNullException(nameof(providers));
            this.hostIdentities = hostIdentities ?? throw new ArgumentNullException(nameof(hostIdentities));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            if (context is null) {
                throw new ArgumentNullException(nameof(context));
            }

            if (next is null) {
                throw new ArgumentNullException(nameof(next));
            }

                       
            var host = context.Request.Host.Host;

#if DEBUG
            if (context.Request.Cookies["vshost"] is object) {
                host = context.Request.Cookies["vshost"].ToString();
            }
#endif

            var identity = default(ClaimsIdentity);

            foreach (var id in hostIdentities) {
                if (id.HasClaim(c => c.Type.Equals(IdClaimTypes.HostIdentifier) && c.Value.Equals(host))) {
                    identity = id;
                    break;
                }
            }

            context.Items["HostIdentity"] = identity ?? hostIdentities.First();
            
            
            HostCultureOptions hostOptions = default;

            foreach (var opt in options.HostOptions) {
                if (Regex.IsMatch(opt.Host, @$"\b{host}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase)) {
                    hostOptions = opt;
                    break;
                }
            }


#if DEBUG
            if (hostOptions is null && context.Request.Cookies["vshost"] is object) {
                context.Response.Cookies.Delete("vshost");
            }
#endif
            ProviderCultureResult requestedCultures = default;

            CultureInfo culture = hostOptions?.DefaultCulture ?? options.DefaultCulture;
            CultureInfo uiCulture = hostOptions?.DefaultUICulture ?? options.DefaultUICulture;

            foreach (var provider in providers) {
                requestedCultures = await provider.DetermineProviderCultureResult(context);
                if (requestedCultures is object) {
                    break;
                }
            }

            if (requestedCultures is object && hostOptions is object) {
                foreach (var supppotedculture in hostOptions.SupportedCultures) {
                    foreach (var requestedCulture in requestedCultures.Cultures) {
                        if (requestedCulture.Equals(supppotedculture.Name, StringComparison.OrdinalIgnoreCase)) {
                            culture = supppotedculture;
                            break;
                        }
                    }
                    if (culture.Equals(supppotedculture)) {
                        break;
                    }
                }

                foreach (var supppoteduiculture in hostOptions.SupportedUICultures) {
                    foreach (var requestedCulture in requestedCultures.UICultures) {
                        if (requestedCulture.Equals(supppoteduiculture.Name, StringComparison.OrdinalIgnoreCase)) {
                            uiCulture = supppoteduiculture;
                            break;
                        }
                    }
                    if (uiCulture.Equals(supppoteduiculture)) {
                        break;
                    }
                }                
            }

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = uiCulture;

            context.Response.Headers.Add(HeaderNames.ContentLanguage, uiCulture.Name);

            await next.Invoke(context);
        }
    }
}