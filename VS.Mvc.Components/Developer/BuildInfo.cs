namespace VS.Mvc.Components.Developer {
    using Microsoft.AspNetCore.Mvc;
    using System.Reflection;
    using VS.Mvc.Components;

    [ViewComponent(Name = "BuildInfo")]
    public class BuildInfo : LocalizedViewComponent {

        public IViewComponentResult Invoke() {

            var assembly = typeof(BuildInfo).Assembly;
            var referencedassemblies = assembly.GetReferencedAssemblies();

            var assemblies = new AssemblyName[referencedassemblies.Length];
            assemblies[0] = assembly.GetName();

            referencedassemblies.CopyTo(assemblies, 0);

            return View(assemblies);
        }
    }
}