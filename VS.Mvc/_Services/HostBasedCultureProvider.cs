namespace VS.Mvc._Services {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;

    public class HostBasedCultureProvider : IRequestCultureProvider {
        private readonly IEnumerable<HostCultureOptions> options;
        private readonly IEnumerable<IRequestCultureProvider> uiProviders;

        public HostBasedCultureProvider(HostCultureOptions[] options, IRequestCultureProvider[] uiProviders) {
            this.options = options?.AsEnumerable() ?? throw new ArgumentNullException(nameof(options));
            this.uiProviders = uiProviders?.AsEnumerable() ?? throw new ArgumentNullException(nameof(uiProviders));
        }

        public async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext) {
            string pattern = @$"\b{httpContext.Request.Host.Host}\b";
            var hostOptions = options.FirstOrDefault(o => Regex.IsMatch(o.Host, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));

            if (hostOptions != null) {

                ProviderCultureResult uiProvider = default;
                var found = false;
                foreach (var provider in uiProviders) {
                    uiProvider = await provider.DetermineProviderCultureResult(httpContext);
                    if (uiProvider is object) {
                        found = true;
                        break;
                    }
                }

                //if (found) {
                //    var matched = hostOptions.SupportedUICultures.Union(uiProvider.UICultures);

                //    if (matched.Any()) {
                //        hostOptions.SupportedUICultures = matched;
                //    }
                //}

                return new ProviderCultureResult(
                    hostOptions.DefaultCulture.Name,
                    hostOptions.DefaultUICulture.Name
                    );

            } else {
                return default;
            }
        }
    }
}