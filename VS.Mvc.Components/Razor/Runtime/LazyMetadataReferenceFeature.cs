using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS.Mvc.Components.Razor.Runtime {
    public class LazyMetadataReferenceFeature : IMetadataReferenceFeature {
        private readonly RazorReferenceManager referenceManager;

        public LazyMetadataReferenceFeature(RazorReferenceManager referenceManager) {
            this.referenceManager = referenceManager;
        }

        /// <remarks>
        /// Invoking <see cref="RazorReferenceManager.CompilationReferences"/> ensures that compilation
        /// references are lazily evaluated.
        /// </remarks>
        public IReadOnlyList<MetadataReference> References => this.referenceManager.CompilationReferences;

        public RazorEngine Engine { get; set; }
    }
}
