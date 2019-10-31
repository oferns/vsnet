namespace VS.Mvc._Extensions {
    using Microsoft.AspNetCore.Mvc.Razor;
    using System.Collections.Generic;
    using System.Linq;

    public class SubAreaViewLocationExpander : IViewLocationExpander {

        private const string _subArea = "subarea";

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations) {
            
            if (context.ActionContext.RouteData.Values.ContainsKey(_subArea)) {
                string subArea = RazorViewEngine.GetNormalizedRouteValue(context.ActionContext, _subArea);

                var subAreaViewLocation = new string[] {
                        "/{2}/"+subArea+"/{1}/{0}" + RazorViewEngine.ViewExtension
                };

                viewLocations = subAreaViewLocation.Concat(viewLocations);

            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context) {
            if (context.ActionContext.ActionDescriptor.RouteValues.TryGetValue(_subArea, out var subArea)) {
                context.Values[_subArea] = subArea;
            }
        }
    }
}