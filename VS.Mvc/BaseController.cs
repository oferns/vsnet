using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace VS.Mvc {

    // This is a base class to provide the core ASP services our contollers need. 
    // It provides:
    //          ModelState
    //          ControllerContext (HttpContext)
    //          SignIn/Out
    public abstract class BaseController : IActionFilter {

        private ControllerContext _controllerContext;
        private ViewDataDictionary _viewData;
        private ITempDataDictionary _tempData;
        private ITempDataDictionaryFactory tempDataDictionaryFactory;

        public ModelStateDictionary ModelState => this.ControllerContext.ModelState;

        // IActionFilter
        [NonAction]
        public void OnActionExecuting(ActionExecutingContext context) {

        }

        [NonAction]
        public void OnActionExecuted(ActionExecutedContext context) {
            var request = context.HttpContext.Request;

            if (request.IsAjax() && request.WantsJson() && context.Result is ViewResult view) {
                context.Result = new ObjectResult(view.Model);
            }

            //if (contentType.Equals("application/json", StringComparison.CurrentCultureIgnoreCase)) {

            //}


        }



        [NonAction]
        public bool IsLocalUrl(string url) {
            if (string.IsNullOrEmpty(url)) {
                return false;
            }

            url = url.Replace(ControllerContext.HttpContext.Request.Scheme + "://" + ControllerContext.HttpContext.Request.Host + ControllerContext.HttpContext.Request.PathBase, "");

            // Allows "/" or "/foo" but not "//" or "/\".
            if (url[0] == '/') {
                // url is exactly "/"
                if (url.Length == 1) {
                    return true;
                }

                // url doesn't start with "//" or "/\"
                if (url[1] != '/' && url[1] != '\\') {
                    return true;
                }

                return false;
            }

            // Allows "~/" or "~/foo" but not "~//" or "~/\".
            if (url[0] == '~' && url.Length > 1 && url[1] == '/') {
                // url is exactly "~/"
                if (url.Length == 2) {
                    return true;
                }

                // url doesn't start with "~//" or "~/\"
                if (url[2] != '/' && url[2] != '\\') {
                    return true;
                }

                return false;
            }

            return false;
        }

        public ITempDataDictionary TempData {
            get {
                tempDataDictionaryFactory = tempDataDictionaryFactory ?? (tempDataDictionaryFactory = this.ControllerContext.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory);

                return this._tempData ?? (this._tempData = this.tempDataDictionaryFactory.GetTempData(this.ControllerContext.HttpContext));
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Mvc.ControllerContext"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Controllers.IControllerActivator"/> activates this property while activating controllers.
        /// If user code directly instantiates a controller, the getter returns an empty
        /// <see cref="Mvc.ControllerContext"/>.
        /// </remarks>
        [ControllerContext]
        public ControllerContext ControllerContext {
            get => this._controllerContext ?? (_controllerContext = new ControllerContext());
            set => _controllerContext = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets <see cref="ViewDataDictionary"/> used by <see cref="ViewResult"/> and <see cref="ViewBag"/>.
        /// </summary>
        /// <remarks>
        /// By default, this property is intiailized when <see cref="Controllers.IControllerActivator"/> activates
        /// controllers.
        /// <para>
        /// This property can be accessed after the controller has been activated, for example, in a controller action
        /// or by overriding <see cref="OnActionExecuting(ActionExecutingContext)"/>.
        /// </para>
        /// <para>
        /// This property can be also accessed from within a unit test where it is initialized with
        /// <see cref="EmptyModelMetadataProvider"/>.
        /// </para>
        /// </remarks>
        [ViewDataDictionary]
        public ViewDataDictionary ViewData {
            get {
                return _viewData;
            }
            set => _viewData = value ?? throw new ArgumentException("ViewDataDictionary", nameof(ViewData));
        }

        [NonAction]
        public async Task SignIn(IIdentity identity, string authenticationScheme, bool allowRefresh, bool persistent) {
            var principal = new ClaimsPrincipal(identity);
            await ControllerContext.HttpContext.SignInAsync(
             authenticationScheme,
             principal,
             new AuthenticationProperties {
                 AllowRefresh = allowRefresh,
                 IsPersistent = persistent,
                 IssuedUtc = DateTime.UtcNow,

             });
            ControllerContext.HttpContext.User = principal;
        }

        [NonAction]
        public async Task SignOut(string authenticationScheme) {
            await ControllerContext.HttpContext.SignOutAsync(authenticationScheme);
        }

        [NonAction]
        public async Task RefreshLocalIdentity(IIdentity identity, string authenticationScheme) {
            await SignOut(authenticationScheme);
            await SignIn(identity, authenticationScheme, true, true); // TODO: Add a claim(?) saying whether they were persistent before or not
        }

        /// <summary>
        /// Creates a <see cref="ViewResult"/> object that renders a view to the response.
        /// </summary>
        /// <returns>The created <see cref="ViewResult"/> object for the response.</returns>
        [NonAction]
        public virtual ViewResult View() {
            return View(viewName: null);
        }

        /// <summary>
        /// Creates a <see cref="ViewResult"/> object by specifying a <paramref name="viewName"/>.
        /// </summary>
        /// <param name="viewName">The name or path of the view that is rendered to the response.</param>
        /// <returns>The created <see cref="ViewResult"/> object for the response.</returns>
        [NonAction]
        public virtual ViewResult View(string viewName) {
            return View(viewName, model: ViewData.Model);
        }

        /// <summary>
        /// Creates a <see cref="ViewResult"/> object by specifying a <paramref name="model"/>
        /// to be rendered by the view.
        /// </summary>
        /// <param name="model">The model that is rendered by the view.</param>
        /// <returns>The created <see cref="ViewResult"/> object for the response.</returns>
        [NonAction]
        public virtual ViewResult View(object model) {
            return View(viewName: null, model: model);
        }

        /// <summary>
        /// Creates a <see cref="ViewResult"/> object by specifying a <paramref name="viewName"/>
        /// and the <paramref name="model"/> to be rendered by the view.
        /// </summary>
        /// <param name="viewName">The name or path of the view that is rendered to the response.</param>
        /// <param name="model">The model that is rendered by the view.</param>
        /// <returns>The created <see cref="ViewResult"/> object for the response.</returns>
        [NonAction]
        public virtual ViewResult View(string viewName, object model) {
            ViewData.Model = model;

            return new ViewResult() {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
        }
    }
}
