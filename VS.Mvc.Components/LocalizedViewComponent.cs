namespace VS.Mvc.Components {

    using System;
    using System.Security.Claims;
    using System.Security.Principal;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Mvc.ViewComponents;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    

    /// <summary>
    /// A base class for view components.
    /// </summary>
    [ViewComponent]
    public abstract class LocalizedViewComponent {

        private dynamic _viewBag;
        private ViewComponentContext _viewComponentContext;
        private ICompositeViewEngine _viewEngine;

        /// <summary>
        /// Gets the <see cref="Http.HttpContext"/>.
        /// </summary>
        public HttpContext HttpContext => ViewContext?.HttpContext;

        /// <summary>
        /// Gets the <see cref="HttpRequest"/>.
        /// </summary>
        public HttpRequest Request => ViewContext?.HttpContext?.Request;

        /// <summary>
        /// Gets the <see cref="IPrincipal"/> for the current user.
        /// </summary>
        public IPrincipal User => ViewContext?.HttpContext?.User;

        /// <summary>
        /// Gets the <see cref="ClaimsPrincipal"/> for the current user.
        /// </summary>
        public ClaimsPrincipal UserClaimsPrincipal => ViewContext?.HttpContext?.User;

        /// <summary>
        /// Gets the <see cref="RouteData"/> for the current request.
        /// </summary>
        public RouteData RouteData => ViewContext?.RouteData;

        /// <summary>
        /// Gets the view bag.
        /// </summary>
        public dynamic ViewBag {
            get {
                if (_viewBag == null) {
                    _viewBag = new DynamicViewData(() => ViewData);
                }

                return _viewBag;
            }
        }

        /// <summary>
        /// Gets the <see cref="ModelStateDictionary"/>.
        /// </summary>
        public ModelStateDictionary ModelState => ViewData?.ModelState;


        [ViewComponentContext]
        public ViewComponentContext ViewComponentContext {
            get { 
                return _viewComponentContext;
            }
            set {
                _viewComponentContext = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets the <see cref="ViewContext"/>.
        /// </summary>
        public ViewContext ViewContext => ViewComponentContext.ViewContext;

        /// <summary>
        /// Gets the <see cref="ViewDataDictionary"/>.
        /// </summary>
        public ViewDataDictionary ViewData => ViewComponentContext.ViewData;

        /// <summary>
        /// Gets the <see cref="ITempDataDictionary"/>.
        /// </summary>
        public ITempDataDictionary TempData => ViewComponentContext.TempData;

        /// <summary>
        /// Gets or sets the <see cref="ICompositeViewEngine"/>.
        /// </summary>
        public ICompositeViewEngine ViewEngine {
            get {
                return _viewEngine;
            }
            set {
                _viewEngine = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Returns a result which will render HTML encoded text.
        /// </summary>
        /// <param name="content">The content, will be HTML encoded before output.</param>
        /// <returns>A <see cref="ContentViewComponentResult"/>.</returns>
        public ContentViewComponentResult Content(string content) {
            if (content == null) {
                throw new ArgumentNullException(nameof(content));
            }

            return new ContentViewComponentResult(content);
        }

        /// <summary>
        /// Returns a result which will render the partial view with name <c>&quot;Default&quot;</c>.
        /// </summary>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public LocalizedViewComponentResult View() {
            return View(viewName: null);
        }

        /// <summary>
        /// Returns a result which will render the partial view with name <paramref name="viewName"/>.
        /// </summary>
        /// <param name="viewName">The name of the partial view to render.</param>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public LocalizedViewComponentResult View(string viewName) {
            return View(viewName, ViewData.Model);
        }

        /// <summary>
        /// Returns a result which will render the partial view with name <c>&quot;Default&quot;</c>.
        /// </summary>
        /// <param name="model">The model object for the view.</param>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public LocalizedViewComponentResult View<TModel>(TModel model) {
            return View(viewName: null, model: model);
        }

        /// <summary>
        /// Returns a result which will render the partial view with name <paramref name="viewName"/>.
        /// </summary>
        /// <param name="viewName">The name of the partial view to render.</param>
        /// <param name="model">The model object for the view.</param>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public LocalizedViewComponentResult View<TModel>(string viewName, TModel model) {
            var viewData = new ViewDataDictionary<TModel>(ViewData, model);
            return new LocalizedViewComponentResult {
                ViewEngine = ViewEngine,
                ViewName = viewName,
                ViewData = viewData
            };
        }
    }
}
