namespace VS.Mvc.Components.Razor.Runtime {

    using Microsoft.Extensions.FileProviders;
    using System.Collections.Generic;

    public class MvcRazorRuntimeCompilationOptions {

        /// <summary>
        /// Gets the <see cref="IFileProvider" /> instances used to locate Razor files.
        /// </summary>
        /// <remarks>
        /// At startup, this collection is initialized to include an instance of
        /// <see cref="IHostingEnvironment.ContentRootFileProvider"/> that is rooted at the application root.
        /// </remarks>
        public IList<IFileProvider> FileProviders { get; } = new List<IFileProvider>();

        /// <summary>
        /// Gets paths to additional references used during runtime compilation of Razor files.
        /// </summary>
        /// <remarks>
        /// By default, the runtime compiler <see cref="ICompilationReferencesProvider"/> to gather references
        /// uses to compile a Razor file. This API allows providing additional references to the compiler.
        /// </remarks>
        public IList<string> AdditionalReferencePaths { get; } = new List<string>();

    }
}
