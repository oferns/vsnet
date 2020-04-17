namespace VS.Mvc._Startup {
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.Razor.Compilation;
    using Microsoft.Extensions.DependencyInjection;
    using SimpleInjector;
    using VS.Mvc._Extensions;

    public static class ViewEngineConfig {

        public static IServiceCollection AddViewOptions(this IServiceCollection services, AntiforgeryOptions antiforgeryOptions) {


            services.AddSingleton<IRazorViewEngine, MultiTenantRazorViewEngine>();
           // services.AddTransient<IRazorPageFactoryProvider, MultiTenantRazorPageFactoryProvider>();
            services.AddTransient<IViewCompilerProvider, MultiTenantViewCompilerProvider>();



            return services.Configure<RazorViewEngineOptions>(options => {
                
                // Clear the defaults
                options.AreaViewLocationFormats.Clear();
                options.ViewLocationFormats.Clear();

                // {2} is the area, {3} is the subarea {1} is the controller, {0} is the action
                options.ViewLocationExpanders.Add(new SubAreaViewLocationExpander());
                options.ViewLocationExpanders.Add(new UICultureViewLocationExpander());


                
                // {2} is area, {1} is controller {0} is the action            
                options.AreaViewLocationFormats.Add("/{2}/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/{2}/{0}" + RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/{0}" + RazorViewEngine.ViewExtension);
                })                
                .AddControllersWithViews(o => {                    
                    o.EnableEndpointRouting = true;                   
                    o.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                })
                
                .ConfigureApplicationPartManager(p => { 
                    p.FeatureProviders.Add(new DataRouteFeatureProvider()); 
                })
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif

                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder).Services
                .AddAntiforgery(o => o = antiforgeryOptions);

        }
    }
}