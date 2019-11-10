namespace VS.Mvc._Startup {
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.DependencyInjection;
    using VS.Mvc._Extensions;

    public static class ViewOptions {

        public static IServiceCollection AddViewOptions(this IServiceCollection services) {

            return services.Configure<RazorViewEngineOptions>(options => {
                    // Clear the defaults
                    options.AreaViewLocationFormats.Clear();
                    options.ViewLocationFormats.Clear();

                    // {2} is the area, {3} is the subarea {1} is the controller, {0} is the action
                    options.ViewLocationExpanders.Add(new SubAreaViewLocationExpander());

                    // {2} is area, {1} is controller {0} is the action            
                    options.AreaViewLocationFormats.Add("/{2}/{1}/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/{2}/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/{0}" + RazorViewEngine.ViewExtension);
                    options.ViewLocationFormats.Add("/{1}/{0}" + RazorViewEngine.ViewExtension);

                    options.ViewLocationFormats.Add("/{0}" + RazorViewEngine.ViewExtension);
                }).AddControllersWithViews(o => {
                   
                    o.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));

                })
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif
                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder).Services;
        }
    }
}