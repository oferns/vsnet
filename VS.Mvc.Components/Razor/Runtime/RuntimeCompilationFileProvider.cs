namespace VS.Mvc.Components.Razor.Runtime {

    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Options;
    using System;

    public class RuntimeCompilationFileProvider {
        private readonly MvcRazorRuntimeCompilationOptions _options;
        private IFileProvider _compositeFileProvider;

        public RuntimeCompilationFileProvider(IOptions<MvcRazorRuntimeCompilationOptions> options) {
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Value;
        }

        public IFileProvider FileProvider {
            get {
                if (_compositeFileProvider == null) {
                    _compositeFileProvider = GetCompositeFileProvider(_options);
                }

                return _compositeFileProvider;
            }
        }

        private static IFileProvider GetCompositeFileProvider(MvcRazorRuntimeCompilationOptions options) {
            var fileProviders = options.FileProviders;
            if (fileProviders.Count == 0) {
                
                throw new InvalidOperationException("No file providers provided");
            } else if (fileProviders.Count == 1) {
                return fileProviders[0];
            }

            return new CompositeFileProvider(fileProviders);
        }
    }
}
