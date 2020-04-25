namespace VS.Mvc.Components.Razor {

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.AspNetCore.Mvc.Razor.Compilation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using VS.Abstractions.Culture;
    using VS.Core.Identity;

    public class MultiTenantViewCompilerProvider : IViewCompilerProvider {
        
        private readonly IHttpContextAccessor contextAccessor;
       

        private readonly IDictionary<string, IViewCompiler> compilers = new Dictionary<string, IViewCompiler>();

        public MultiTenantViewCompilerProvider(ApplicationPartManager applicationPartManager, IHttpContextAccessor contextAccessor,  CultureOptions cultureOptions) {

            if (applicationPartManager is null) {
                throw new ArgumentNullException(nameof(applicationPartManager));
            }

            if (cultureOptions is null) {
                throw new ArgumentNullException(nameof(cultureOptions));
            }

            this.contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));

            var feature = new ViewsFeature();
            applicationPartManager.PopulateFeature(feature);
                       
            var defaultViews = new List<CompiledViewDescriptor>();

            foreach (var descriptor in feature.ViewDescriptors) {
                if (!defaultViews.Any(v => v.RelativePath.Equals(descriptor.RelativePath))) {
                    defaultViews.Add(descriptor);
                }
            }

            compilers.Add("default", new MultiTenantViewCompiler(defaultViews));


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
             if (contextAccessor.HttpContext.Items["HostIdentity"] is ClaimsIdentity hostId) {
                var host = hostId.FindFirst(IdClaimTypes.HostIdentifier);
                if (host is object && compilers.ContainsKey(host.Value)) {
                    return compilers[host.Value];
                }
            }
            return compilers["default"];
        }
    }
}
