namespace VS.Mvc.Components.Razor.Runtime {

    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.Razor.Compilation;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class MultiTenantRuntimePageFactoryProvider : IRazorPageFactoryProvider {

        private readonly IViewCompilerProvider viewCompilerProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultRazorPageFactoryProvider"/>.
        /// </summary>
        /// <param name="viewCompilerProvider">The <see cref="IViewCompilerProvider"/>.</param>
        public MultiTenantRuntimePageFactoryProvider(IViewCompilerProvider viewCompilerProvider) {
            this.viewCompilerProvider = viewCompilerProvider;
        }

        private IViewCompiler Compiler => this.viewCompilerProvider.GetCompiler();

        /// <inheritdoc />
        public RazorPageFactoryResult CreateFactory(string relativePath) {
            if (relativePath == null) {
                throw new ArgumentNullException(nameof(relativePath));
            }

            if (relativePath.StartsWith("~/", StringComparison.Ordinal)) {
                // For tilde slash paths, drop the leading ~ to make it work with the underlying IFileProvider.
                relativePath = relativePath.Substring(1);
            }

            var compileTask = Compiler.CompileAsync(relativePath);
            var viewDescriptor = compileTask.GetAwaiter().GetResult();

            var viewType = viewDescriptor.Type;
            if (viewType != null) {
                var newExpression = Expression.New(viewType);
                var pathProperty = viewType.GetTypeInfo().GetProperty(nameof(IRazorPage.Path));

                // Generate: page.Path = relativePath;
                // Use the normalized path specified from the result.
                var propertyBindExpression = Expression.Bind(pathProperty, Expression.Constant(viewDescriptor.RelativePath));
                var objectInitializeExpression = Expression.MemberInit(newExpression, propertyBindExpression);
                var pageFactory = Expression
                    .Lambda<Func<IRazorPage>>(objectInitializeExpression)
                    .Compile();
                return new RazorPageFactoryResult(viewDescriptor, pageFactory);
            } else {
                return new RazorPageFactoryResult(viewDescriptor, razorPageFactory: null);
            }
        }
    }
}