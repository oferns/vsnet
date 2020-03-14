namespace VS.Mvc.RegisterCard {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using VS.Core.Identity;
    using VS.Core.Payment;

    [Controller]
    public class RegisterCardController {
        private readonly IMediator mediator;
        private readonly IActionContextAccessor actionContext;

        public RegisterCardController(IMediator mediator, IActionContextAccessor actionContext) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.actionContext = actionContext ?? throw new ArgumentNullException(nameof(actionContext));
        }



        [HttpGet]
        public IActionResult Index() {
            var userIdClaim = actionContext.ActionContext.HttpContext.User.FindFirst(IdClaimTypes.UserIdentifier);
            actionContext.ActionContext.RouteData.Values.Add("userId", userIdClaim.Value);
            return new ViewResult { ViewName = "RegisterCard" };
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string registrationId, CancellationToken cancel) {

            var success = await mediator.Send(new RemoveRegisteredCardRequest(registrationId, Guid.Empty.ToString()), cancel);

            return new RedirectToActionResult("Index", "RegisterCard", null);

        }




        [HttpGet]
        public async Task<IActionResult> Callback(string id, string resourcePath, CancellationToken cancel) {

            this.ViewData.Model = await mediator.Send(new RegisterCardStatusRequest(id), cancel);

            return new RedirectToActionResult("Index", "RegisterCard", null);
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
                    _viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), actionContext.ActionContext.ModelState);
                }

                return _viewData;
            }
            set {
                _viewData = value ?? throw new ArgumentException("ViewData cannot be null.", nameof(ViewData));
            }
        }

    }
}
