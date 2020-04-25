namespace VS.Mvc.Components.Razor.Runtime {

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class MultiTenantRuntimeCompilationOptionsSetup : IConfigureOptions<MultiTenantRuntimeCompilationOptions> {
        private readonly IWebHostEnvironment hostingEnvironment;

        public MultiTenantRuntimeCompilationOptionsSetup(IWebHostEnvironment hostingEnvironment) {
            this.hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }


        public void Configure(MultiTenantRuntimeCompilationOptions options) {
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            var assembly = Assembly.GetEntryAssembly().GetName().Name + ".Views";

            var defaultoptions = new MvcRazorRuntimeCompilationOptions();
            defaultoptions.FileProviders.Add(this.hostingEnvironment.ContentRootFileProvider);

            options.Prepend(KeyValuePair.Create(assembly, defaultoptions));

        }
    }
}
