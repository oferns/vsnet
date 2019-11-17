namespace VS.Mvc._Middleware {
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Net.Http.Headers;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using VS.Mvc._Services;

    public class HostBasedLocalizationMiddleware : IMiddleware {

        private readonly CultureOptions options;
        private readonly IEnumerable<IRequestCultureProvider> providers;

        public HostBasedLocalizationMiddleware(CultureOptions options, IEnumerable<IRequestCultureProvider> providers) {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.providers = providers ?? throw new ArgumentNullException(nameof(providers));
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

            var hostOptions = options.HostOptions.FirstOrDefault(o => Regex.IsMatch(o.Host, @$"\b{host}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase));

#if DEBUG
            if (hostOptions is null) {
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
                culture = hostOptions.SupportedCultures.Where(s => requestedCultures.Cultures.Contains(s.Name)).FirstOrDefault() ?? culture;
                uiCulture = hostOptions.SupportedUICultures.Where(s => requestedCultures.UICultures.Contains(s.Name)).FirstOrDefault() ?? uiCulture;
            }

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = uiCulture;

            context.Response.Headers.Add(HeaderNames.ContentLanguage, uiCulture.Name);

            await next.Invoke(context);
        }
    }
}