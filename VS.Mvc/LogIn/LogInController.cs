namespace VS.Mvc.LogIn {

    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Core.Identity;
    using VS.Mvc._Extensions;
    using VS.Mvc.Components.LogIn;

    [Controller]
    public class LogInController {
        private readonly IMediator mediator;
        private readonly IActionContextAccessor contextAccessor;

        public LogInController(IMediator mediator, IActionContextAccessor contextAccessor) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        [HttpGet]
        [NavLink("User", "Log In", 0)]
        public IActionResult Index() => new ViewResult { ViewName = "LogIn" };

        [HttpPost]
        [ValidateAntiForgeryToken]                                                                                                                                                                                                                                                   
        [FormAction(LogInForm.Name, "Log In", 0)]
        public async Task<IActionResult> Index(LoginWithPassword loginRequest, CancellationToken cancel) {
            if (ModelState.IsValid && loginRequest is object) {
                // do something here
                var identity = await mediator.Send(loginRequest, cancel);
                
                if (identity is object) {
                    await contextAccessor.ActionContext.SignInAsync(identity, "", true, loginRequest.Persistent, cancel);
                    // TODO: Flash a message
                    return new RedirectToActionResult("Index", "Home", null);
                }

            }

            // TODO: Flash a message

            return new ViewResult { ViewName = "LogIn" };
        }


        private ModelStateDictionary ModelState => contextAccessor.ActionContext.ModelState;

    }
}
