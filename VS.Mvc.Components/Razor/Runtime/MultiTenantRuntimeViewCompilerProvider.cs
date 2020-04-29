namespace VS.Mvc.Components.Razor.Runtime {
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.AspNetCore.Mvc.Razor.Compilation;
    using Microsoft.AspNetCore.Razor.Language;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using VS.Abstractions.Culture;
    using VS.Core.Identity;

    public class MultiTenantRuntimeViewCompilerProvider : IViewCompilerProvider {

        private readonly IHttpContextAccessor contextAccessor;
        private readonly CultureOptions cultureOptions;        
        private readonly ILogger<MultiTenantRuntimeViewCompiler> logger;
        

        private readonly IDictionary<string, IViewCompiler> compilers = new Dictionary<string, IViewCompiler>();


        public MultiTenantRuntimeViewCompilerProvider(IHttpContextAccessor contextAccessor,
                                                    ApplicationPartManager applicationPartManager,
                                                    CultureOptions cultureOptions,
                                                    IDictionary<string, RazorProjectEngine> razorProjectEngines,                                                    
                                                    CSharpCompiler csharpCompiler,
                                                    ILoggerFactory loggerFactory) {
            
            this.contextAccessor = contextAccessor;
            this.cultureOptions = cultureOptions;

            
            logger = loggerFactory.CreateLogger<MultiTenantRuntimeViewCompiler>();

            var feature = new ViewsFeature();
            applicationPartManager.PopulateFeature(feature);

            var defaultViews = new List<CompiledViewDescriptor>();

            foreach (var descriptor in feature.ViewDescriptors) {
                if (!defaultViews.Exists(v => v.RelativePath.Equals(descriptor.RelativePath, StringComparison.OrdinalIgnoreCase))) {
                    defaultViews.Add(descriptor);
                }
            }

            compilers.Add("default", new MultiTenantRuntimeViewCompiler(                
                razorProjectEngines,
                csharpCompiler,
                defaultViews,
                logger));

            foreach (var host in this.cultureOptions.HostOptions) {
                var viewDescriptors = new List<CompiledViewDescriptor>();

                var hostSpecificViews = feature.ViewDescriptors.Where(d => d.Item.Type.Assembly.GetName().Name.Equals(host.ViewLibrary));

                foreach (var view in hostSpecificViews) {
                    if (!viewDescriptors.Any(v => v.RelativePath.Equals(view.RelativePath, StringComparison.OrdinalIgnoreCase))) {
                        viewDescriptors.Add(view);
                    }

                }

                foreach (var view in defaultViews) {
                    if (!viewDescriptors.Any(v => v.RelativePath.Equals(view.RelativePath))) {
                        viewDescriptors.Add(view);
                    }
                }
                
                compilers.Add(host.Host, new MultiTenantRuntimeViewCompiler(
                          razorProjectEngines,
                          csharpCompiler,
                          viewDescriptors.ToList(),
                          logger));
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
