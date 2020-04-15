namespace VS.Mvc._Extensions {

    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewComponents;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.DependencyInjection;

    public class LocalizedViewComponentResult : IViewComponentResult {

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ViewDataDictionary"/>.
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ITempDataDictionary"/> instance.
        /// </summary>
        public ITempDataDictionary TempData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ViewEngine"/>.
        /// </summary>
        public IViewEngine ViewEngine { get; set; }

        /// <summary>
        /// Locates and renders a view specified by <see cref="ViewName"/>. If <see cref="ViewName"/> is <c>null</c>,
        /// then the view name searched for is<c>&quot;Default&quot;</c>.
        /// </summary>
        /// <param name="context">The <see cref="ViewComponentContext"/> for the current component execution.</param>
        /// <remarks>
        /// This method synchronously calls and blocks on <see cref="ExecuteAsync(ViewComponentContext)"/>.
        /// </remarks>
        public void Execute(ViewComponentContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            var task = ExecuteAsync(context);
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            task.GetAwaiter().GetResult();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
        }

        /// <summary>
        /// Locates and renders a view specified by <see cref="ViewName"/>. If <see cref="ViewName"/> is <c>null</c>,
        /// then the view name searched for is<c>&quot;Default&quot;</c>.
        /// </summary>
        /// <param name="context">The <see cref="ViewComponentContext"/> for the current component execution.</param>
        /// <returns>A <see cref="Task"/> which will complete when view rendering is completed.</returns>
        public async Task ExecuteAsync(ViewComponentContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            var viewEngine = ViewEngine ?? ResolveViewEngine(context);
            var viewContext = context.ViewContext;

            var componentName = ViewName ?? context.ViewComponentDescriptor.TypeInfo.Name;
            var culturedComponentName = $"{componentName}.{CultureInfo.CurrentCulture.Name}";
            var baseNs = context.ViewComponentDescriptor.TypeInfo.Assembly.GetName().Name;
            var viewPath = context.ViewComponentDescriptor.TypeInfo.FullName.Replace(baseNs, string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(componentName, string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace('.', '/');

            var culturedPath = Path.Combine(viewPath, culturedComponentName) + ".cshtml";
            var basePath = Path.Combine(viewPath, componentName) + ".cshtml";

            ViewEngineResult result = viewEngine.GetView(viewPath, culturedPath, isMainPage: false);

            if (result == null || !result.Success) {
                result = viewEngine.GetView(viewPath, basePath, isMainPage: false);

            } else {
                ViewName = culturedComponentName;
            }

            if (result == null || !result.Success) {
                result = viewEngine.FindView(viewContext, culturedPath, isMainPage: false);
            } else {
                ViewName = componentName;
            }

            if (result == null || !result.Success) {
                result = viewEngine.FindView(viewContext, basePath, isMainPage: false);
            } else {
                ViewName = culturedComponentName;
            }

            //if (result == null || !result.Success) {
            //    throw new 
            //}

            using (result.View as IDisposable) {

                var childViewContext = new ViewContext(
                    viewContext,
                    result.View,
                    ViewData ?? context.ViewData,
                    context.Writer);
                await result.View.RenderAsync(childViewContext).ConfigureAwait(false);

            }
        }

        private static IViewEngine ResolveViewEngine(ViewComponentContext context) {
            return context.ViewContext.HttpContext.RequestServices.GetRequiredService<ICompositeViewEngine>();
        }
    }
}