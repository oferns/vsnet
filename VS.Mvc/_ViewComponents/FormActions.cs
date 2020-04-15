namespace VS.Mvc._ViewComponents {
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Mvc._Extensions;

    [ViewComponent(Name="FormActions")]
    public class FormActions : LocalizedViewComponent {
        private readonly IActionProvider actionProvider;

        public FormActions(IActionProvider actionProvider) {
            this.actionProvider = actionProvider ?? throw new ArgumentNullException(nameof(actionProvider));
        }

        public async Task<IViewComponentResult> InvokeAsync(string formName) {

            var actions = await actionProvider.Actions(formName, ViewContext.HttpContext.RequestAborted).ConfigureAwait(false);
            var model = new FormActionsModel(actions);

            return View(model);
        }
    }
}
