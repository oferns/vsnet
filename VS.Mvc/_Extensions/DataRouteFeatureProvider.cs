namespace VS.Mvc._Extensions {

    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using SimpleInjector;
    using System.Collections.Generic;
    using VS.Abstractions.Data;

    public class DataRouteFeatureProvider : IApplicationFeatureProvider<ControllerFeature> {
       
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature) {

                         //   feature.Controllers.


        }
    }
}
