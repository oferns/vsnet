namespace VS.Mvc.Error {
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    [AllowAnonymous]
    [Controller]
    public class ErrorController {
        private readonly IActionContextAccessor contextAccessor;

        public ErrorController(IActionContextAccessor contextAccessor) {
            this.contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public IActionResult Index(int? sc) {
            var status = sc ?? 500;

            switch (status) {
                case 500:
                    var exceptionAccessor = contextAccessor.ActionContext.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                    this.ViewData.Model = exceptionAccessor?.Error ?? new ApplicationException("Unspecified Error!");
                    return new ViewResult { ViewName = "500", ViewData = this.ViewData };
                default:
                case 404:
                    return new ViewResult { ViewName = "404" };
                case 400:
                    return new BadRequestResult();
            }

        }


        private ViewDataDictionary _viewData;

        /// <summary>
        /// Gets or sets <see cref="ViewDataDictionary"/> used by <see cref="ViewResult"/> and <see cref="ViewBag"/>.
        /// </summary>
        /// <remarks>
        /// By default, this property is initialized when <see cref="Controllers.IControllerActivator"/> activates
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
                if (_viewData == null) {
                    // This should run only for the controller unit test scenarios
                    _viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), contextAccessor.ActionContext.ModelState);
                }

                return _viewData;
            }
            set {
                _viewData = value ?? throw new ArgumentException("ViewData cannot be null.", nameof(ViewData));
            }
        }
    }
}
