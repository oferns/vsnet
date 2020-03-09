namespace VS.Mvc.RegisterCard._forms {
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using VS.Core.Payment;
    using VS.Mvc._Extensions;


    [ViewComponent(Name = "RegisterCardForm")]
    public class RegisterCardForm : LocalizedViewComponent {
        private readonly IMediator mediator;
        private readonly LinkGenerator link;
        

        public RegisterCardForm(IMediator mediator, LinkGenerator link) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.link = link ?? throw new ArgumentNullException(nameof(link));
        }



        public async Task<IViewComponentResult> InvokeAsync() {

            var response = await mediator.Send(new RegisterCardRequest());

            if (response.Success.Equals(false)) {
                return View();
            }


            var returnUri = link.GetUriByAction(ViewContext.HttpContext, "Callback", "RegisterCard");

            var model = new RegisterCardFormModel {
                OppWaId = response.ProviderReference,
                CardProviders = new[] { "Visa", "MasterCard" },
                CardFormAction = new Uri($"https://test.oppwa.com/v1/checkouts/{response.ProviderReference}"),
                FormAction = new Uri($"https://test.oppwa.com/v1/checkouts/{response.ProviderReference}"),
                ReturnUrl = new Uri(returnUri)
            };


            return View(model);
        }

    }
}
