namespace VS.Mvc.Components.Razor.Runtime {

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Options;
    using System;
    using System.IO;
    using System.Reflection;

    public class MvcRazorRuntimeCompilationOptionsSetup : IConfigureOptions<MvcRazorRuntimeCompilationOptions> {
        private readonly IWebHostEnvironment hostingEnvironment;

        public MvcRazorRuntimeCompilationOptionsSetup(IWebHostEnvironment hostingEnvironment) {
            this.hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        public void Configure(MvcRazorRuntimeCompilationOptions options) {
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            // options.FileProviders.Add(new PhysicalFileProvider(Path.Combine(hostingEnvironment.ContentRootPath, "..", "VS.Mvc.Vivastreet_Com")));
            options.FileProviders.Add(this.hostingEnvironment.ContentRootFileProvider);

        }
    }
}
