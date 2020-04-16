
namespace VS.Mvc._Extensions {
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.AspNetCore.Mvc.Razor.Compilation;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Core.Identity;
    using VS.Mvc._Services;

    public class MultiTenantViewCompilerProvider : IViewCompilerProvider {
        private readonly ApplicationPartManager applicationPartManager;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly CultureOptions cultureOptions;


        private readonly IDictionary<string, IViewCompiler> compilers = new Dictionary<string, IViewCompiler>();

        public MultiTenantViewCompilerProvider(
            ApplicationPartManager applicationPartManager,
            IHttpContextAccessor contextAccessor,            
            CultureOptions cultureOptions) {

            this.applicationPartManager = applicationPartManager ?? throw new ArgumentNullException(nameof(applicationPartManager));
            this.contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            this.cultureOptions = cultureOptions ?? throw new ArgumentNullException(nameof(cultureOptions));

            var feature = new ViewsFeature();
            applicationPartManager.PopulateFeature(feature);

            var defaultViews = feature.ViewDescriptors.Where(d => d.Item.Type.Assembly.GetName().Name.Equals($"{this.GetType().Assembly.GetName().Name}.Views"));

            compilers.Add("default", new MultiTenantViewCompiler(defaultViews.ToList()));

            var viewLibs = applicationPartManager.ApplicationParts.Where(p => p is CompiledRazorAssemblyPart).Select(p => p as CompiledRazorAssemblyPart);
            var viewFeatures = applicationPartManager.ApplicationParts.OfType<IRazorCompiledItemProvider>();


            foreach (var host in cultureOptions.HostOptions) {

                var viewDescriptors = new List<CompiledViewDescriptor>();

                var hostSpecificViews = feature.ViewDescriptors.Where(d => d.Item.Type.Assembly.GetName().Name.Equals(host.ViewLibrary));

                foreach (var view in hostSpecificViews) {
                    if (!viewDescriptors.Any(v => v.RelativePath.Equals(view.RelativePath))) {
                        viewDescriptors.Add(view);                                                                                                  
                    }

                }

                foreach (var view in defaultViews) {
                    if (!viewDescriptors.Any(v => v.RelativePath.Equals(view.RelativePath))) {
                        viewDescriptors.Add(view);
                    }
                }

                compilers.Add(host.Host, new MultiTenantViewCompiler(viewDescriptors));
            }
        }


        public IViewCompiler GetCompiler() {            
             if (contextAccessor.HttpContext.Items["HostUdentity"] is ClaimsIdentity hostId) {
                var host = hostId.FindFirst(IdClaimTypes.HostIdentifier);
                if (host is object && compilers.ContainsKey(host.Value)) {
                    return compilers[host.Value];
                }
            }
            return compilers["default"];
        }
    }
}
