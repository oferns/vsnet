namespace VS.Mvc.Components.Error {
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.Localization;
    using System;
    using VS.Mvc.Components;

    [ViewComponent(Name = "ExceptionView")]
    public class ExceptionView : LocalizedViewComponent {

        private readonly IActionContextAccessor contextAccessor;
        private readonly IStringLocalizer localizer;

        public ExceptionView(IActionContextAccessor contextAccessor, IStringLocalizer localizer) {
            this.contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public IViewComponentResult Invoke() {
            var exceptionAccessor = contextAccessor.ActionContext.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            return View<Exception>(exceptionAccessor?.Error ?? new ApplicationException(localizer["Unspecified Error!"]));
        }
    }
}