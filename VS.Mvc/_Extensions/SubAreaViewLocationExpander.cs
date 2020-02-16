namespace VS.Mvc._Extensions {
    using Microsoft.AspNetCore.Mvc.Razor;
    using System.Collections.Generic;

    public class SubAreaViewLocationExpander : IViewLocationExpander {

        private const string _subArea = "subarea";

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations) {

            var list = new List<string>(viewLocations);
            if (context.ActionContext.RouteData.Values.ContainsKey(_subArea)) {
                string subArea = RazorViewEngine.GetNormalizedRouteValue(context.ActionContext, _subArea);

                list.Add("/{2}/" + subArea + "/{1}/{0}" + RazorViewEngine.ViewExtension);                
            }
            return list;
        }

        public void PopulateValues(ViewLocationExpanderContext context) {
            if (context.ActionContext.ActionDescriptor.RouteValues.TryGetValue(_subArea, out var subArea)) {
                context.Values[_subArea] = subArea;
            }
        }
    }
}