

namespace VS.Mvc._Extensions {
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Localization;

    public class LocalizedRouteValueTransformer : DynamicRouteValueTransformer {
        private readonly IStringLocalizer localizer;

        public LocalizedRouteValueTransformer(IStringLocalizer localizer) {
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values) {

             var returnedValues = new RouteValueDictionary();
            if (values["subarea"] is string subarea) {
                returnedValues["subarea"] = localizer[subarea].Value;

            }

            if (values["area"] is string area) {
                returnedValues["area"] = localizer[area].Value;
                    
            }

            if (values["controller"] is string controller) {
                returnedValues["controller"] = localizer[controller].Value;

            } else {
                returnedValues["controller"] = "Home";
            }

            if (values["action"] is string action) {
                returnedValues["action"] = localizer[action].Value;
            } else {
                returnedValues["action"] = "Index";
            }



            return returnedValues;
        }
    }
}
