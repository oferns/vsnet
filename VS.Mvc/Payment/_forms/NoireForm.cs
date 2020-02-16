namespace VS.Mvc.Payment._forms {
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using VS.Core.Data;
    using VS.Data.PostGres.App;
    using VS.Mvc._Extensions;

    [ViewComponent(Name="NoireForm")]
    public class NoireForm : LocalizedViewComponent {
        
        private readonly IMediator mediator;

        public NoireForm(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var stamp = await mediator.Send(new GetOne<SecurityStamp>());
            var model = new NoireFormViewModel { 
                CardProviders = new[] { "Visa", "MasterCard" },
                FormAction = new Uri($"test://test/{stamp.Stamp}"),
                ReturnUrl = new Uri($"test://test/{stamp.Stamp}")
            };

            return View(model);
        }
    }
}