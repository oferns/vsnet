namespace VS.Mvc._Middleware {
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using VS.Mvc._Services;

    public class HostBasedLocalizationMiddleware : IMiddleware {

        private readonly HostCultureOptions[] options;
        private readonly IEnumerable<IRequestCultureProvider> providers;

        public HostBasedLocalizationMiddleware(HostCultureOptions[] options, IEnumerable<IRequestCultureProvider> providers) {
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

            string pattern = @$"\b{context.Request.Host.Host}\b";
            var hostOptions = options.FirstOrDefault(o => Regex.IsMatch(o.Host, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));

            ProviderCultureResult requestedCultures = default;

            CultureInfo culture = hostOptions.DefaultCulture;
            CultureInfo uiCulture = hostOptions.DefaultUICulture;

            foreach (var provider in providers) {
                requestedCultures = await provider.DetermineProviderCultureResult(context);
                if (requestedCultures is object) {
                    break;
                }
            }

            if (requestedCultures is object) {
                culture = hostOptions.SupportedCultures.Where(s => requestedCultures.Cultures.Contains(s.Name)).FirstOrDefault() ?? culture;
                uiCulture = hostOptions.SupportedUICultures.Where(s => requestedCultures.UICultures.Contains(s.Name)).FirstOrDefault() ?? uiCulture;
            }

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = uiCulture;

            await next.Invoke(context);
        }
    }
}