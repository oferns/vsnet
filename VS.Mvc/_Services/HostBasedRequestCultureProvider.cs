
namespace VS.Mvc.Services {
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HostBasedRequestCultureProvider : IRequestCultureProvider {
        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext) {

            return Task.FromResult(new ProviderCultureResult("fr-FR", "fr-FR"));

        }
    }
}
