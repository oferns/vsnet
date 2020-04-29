namespace VS.Mvc.Components.RegisterCard {
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using VS.Abstractions;
    using VS.Core.Payment;
    using VS.Mvc.Components;

    [ViewComponent(Name = "RegisteredCardsList")]
    public class RegisteredCardsList : LocalizedViewComponent {

        private readonly IMediator mediator;
        private readonly IContext context;

        public RegisteredCardsList(IMediator mediator, IContext context) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var registeredCards = await mediator.Send(new RegisteredCardsRequest(Guid.Empty.ToString()));
            return View(registeredCards);
        }
    }
}