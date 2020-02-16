namespace VS.Mvc._Extensions {

    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc.Razor;

    public class UICultureViewLocationExpander : IViewLocationExpander {


        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations) {
            var temporaryCultureInfo = CultureInfo.CurrentUICulture;
            foreach (var location in viewLocations) {
                var localparts = location.Split("/");
                while (temporaryCultureInfo != temporaryCultureInfo.Parent) {
                    var parts = temporaryCultureInfo.Name.Split('-');

                    yield return location.Replace(localparts[^1], "_" + string.Join('/', parts) + "/" + localparts[^1]);
                    temporaryCultureInfo = temporaryCultureInfo.Parent;
                }
                yield return location;
            }
        }

        public void PopulateValues(ViewLocationExpanderContext context) {
            // Nothing to see here
        }
    }
}
