namespace VS.Mvc._Extensions {
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class KnownTermsRouteValuesTransformer : DynamicRouteValueTransformer {


        public KnownTermsRouteValuesTransformer(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        private static readonly IEnumerable<string> Locations = new [] { "London", "Brighton" };
        private static readonly IEnumerable<string> Categories = new [] { "Adult", "Jobs", "Furniture" };
        private readonly IMediator mediator;

        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values) {
            if (httpContext is null) {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (values is null) {
                throw new ArgumentNullException(nameof(values));
            }

            var returnedValues = new RouteValueDictionary();
            if (values.ContainsKey("url")) {

                var parts = values["url"].ToString().Split("/", StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length.Equals(0)) {
                    return new ValueTask<RouteValueDictionary>(returnedValues);
                }


                var searchTerms = new StringBuilder();
                var hasLocation = false;
                var hasCategory = false;

                foreach (var part in parts) {
                    foreach (var location in Locations) {
                        if (location.Equals(part, StringComparison.OrdinalIgnoreCase) && hasLocation.Equals(false)) {
                            searchTerms.Append($"{part} ");
                            hasLocation = true;
                            break;
                        }
                    }

                    foreach (var category in Categories) {

                        if (category.Equals(part, StringComparison.OrdinalIgnoreCase) && hasCategory.Equals(false)) {
                            searchTerms.Append($"{part} ");
                            hasCategory = true;
                            break;
                        }
                    }
                }

                if (searchTerms.Length > 0) {
                    returnedValues["controller"] = "Search";
                    returnedValues["action"] = "Index";
                    returnedValues["q"] = searchTerms;
                }
            }


            return new ValueTask<RouteValueDictionary>(returnedValues);
        }

    }
}
