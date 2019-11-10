namespace VS.Mvc._Components {
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "BuildInfo")]
    public class BuildInfo : LocalizedViewComponent {

        public IViewComponentResult Invoke() {

            var assembly = typeof(BuildInfo).Assembly;
            var model = assembly.GetReferencedAssemblies().ToList();
            model = model.Prepend(assembly.GetName()).ToList();
            return View(model.ToArray());   
        }	
    }
}