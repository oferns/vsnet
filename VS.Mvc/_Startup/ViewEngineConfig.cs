namespace VS.Mvc._Startup {
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.AspNetCore.Mvc.Localization;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.Razor.Compilation;
    using Microsoft.AspNetCore.Mvc.Razor.Extensions;
    using Microsoft.AspNetCore.Razor.Hosting;
    using Microsoft.AspNetCore.Razor.Language;
    using Microsoft.CodeAnalysis.Razor;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualBasic;
    using SimpleInjector;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using VS.Abstractions.Culture;
    using VS.Mvc._Extensions;
    using VS.Mvc.Components.Razor;
    using VS.Mvc.Components.Razor.Runtime;

    public static class ViewEngineConfig {


        private static IDictionary<string, RazorProjectEngine> GetEnginesForAssembly(Assembly rootAssembly, CSharpCompiler csharpCompiler, RazorProjectFileSystem projectFileSystem, RazorReferenceManager referenceManager) {

            var dictionary = new Dictionary<string, RazorProjectEngine>();
            var relatedAttribute = rootAssembly.GetCustomAttributes<RelatedAssemblyAttribute>();

            var relatedViewAssemblies = new List<Assembly>();
            foreach (var att in relatedAttribute) {
                var relatedAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.Equals(att.AssemblyFileName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (relatedAssembly is object) {
                    if (relatedAssembly.CustomAttributes.Any(c => c.AttributeType.Equals(typeof(RazorCompiledItemAttribute)))) {
                                                
                        var engineConfig = RazorConfiguration.Create(RazorLanguageVersion.Latest, att.AssemblyFileName, Array.Empty<RazorExtension>());
                        var engine = RazorProjectEngine.Create(engineConfig, projectFileSystem, builder => {
                            RazorExtensions.Register(builder);

                            // Roslyn + TagHelpers infrastructure                            
                            builder.Features.Add(new LazyMetadataReferenceFeature(referenceManager));
                            builder.Features.Add(new CompilationTagHelperFeature());

                            // TagHelperDescriptorProviders (actually do tag helper discovery)
                            builder.Features.Add(new DefaultTagHelperDescriptorProvider());
                            builder.Features.Add(new ViewComponentTagHelperDescriptorProvider());
                            builder.SetCSharpLanguageVersion(csharpCompiler.ParseOptions.LanguageVersion);
                        });

                        dictionary.Add(att.AssemblyFileName, engine);
                    }
                }
            }

            return dictionary;

        }

        public static IServiceCollection AddViewOptions(this IServiceCollection services, AntiforgeryOptions antiforgeryOptions, CultureOptions cultureOptions, IWebHostEnvironment environment) {

            services.AddTransient<IViewLocalizer, ViewLocalizer>();
            services.AddTransient<IHtmlLocalizerFactory, HtmlLocalizerFactory>();
            services.AddTransient<IRazorPageFactoryProvider, MultiTenantRuntimePageFactoryProvider>();

            //services.AddSingleton<IRazorViewEngine, MultiTenantRazorViewEngine>();
            //services.AddSingleton<IViewCompilerProvider, MultiTenantViewCompilerProvider>();

            services.AddSingleton<IViewCompilerProvider, MultiTenantRuntimeViewCompilerProvider>();
            services.AddSingleton<IRazorViewEngine, MultiTenantRazorViewEngine>();
            services.AddSingleton<RazorReferenceManager>();
            services.AddSingleton<CSharpCompiler>();

            services.AddSingleton<IDictionary<string, RazorProjectEngine>>(s => {

                var csharpCompiler = s.GetRequiredService<CSharpCompiler>();
                var referenceManager = s.GetRequiredService<RazorReferenceManager>();
                var dictionary = new Dictionary<string, RazorProjectEngine>();

                var entryAssembly = Assembly.GetEntryAssembly();
                var setBase = false;

                // Add Razor projects
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.CustomAttributes.Any(c => c.AttributeType.Equals(typeof(RelatedAssemblyAttribute))))) {
                    
                    if (assembly.Equals(entryAssembly)) {
                        setBase = true;
                        continue; // Add this as the base library
                    }

                    var customRelativePath = cultureOptions.HostOptions.Where(h => h.ViewLibrary.Equals(assembly.GetName().Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    var relativePath = customRelativePath?.RelativePathToViewLibrarySource ?? $"../{assembly.GetName().Name}";
                    var path = Path.GetFullPath(Path.Combine(environment.ContentRootPath, relativePath));
                    var projectFileSystem = new FileProviderRazorProjectFileSystem(path);

                    var engines = GetEnginesForAssembly(assembly, csharpCompiler, projectFileSystem, referenceManager);

                    foreach (var engine in engines) {
                        if (!dictionary.ContainsKey(engine.Key)) {
                            dictionary.Add(engine.Key, engine.Value);
                        }
                    }
                }

                if (setBase) {
                    var engines = GetEnginesForAssembly(entryAssembly, csharpCompiler, new FileProviderRazorProjectFileSystem(environment.ContentRootPath), referenceManager);
                    foreach (var engine in engines) {
                        if (!dictionary.ContainsKey(engine.Key)) {
                            dictionary.Add(engine.Key, engine.Value);
                        }
                    }
                }

                return dictionary;
            });

            return services.Configure<RazorViewEngineOptions>(options => {

                // Clear the defaults
                options.AreaViewLocationFormats.Clear();
                options.ViewLocationFormats.Clear();
                options.ViewLocationExpanders.Clear();

                options.ViewLocationExpanders.Add(new SubAreaViewLocationExpander());
                options.ViewLocationExpanders.Add(new UICultureViewLocationExpander());

                // {2} is area, {1} is controller {0} is the action            
                options.AreaViewLocationFormats.Add("/{2}/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/{2}/{0}" + RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/{0}" + RazorViewEngine.ViewExtension);

                // {1} is controller {0} is the action
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

                .AddDataAnnotationsLocalization().Services
                .AddAntiforgery(o => o = antiforgeryOptions);

        }
    }
}