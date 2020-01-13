namespace VS.Mvc.DevGuide.Examples.Messaging._Lists {
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using VS.Core.Storage;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "ExampleMessageList")]
    public class ExampleMessageList : LocalizedViewComponent {
        private readonly IMediator mediator;

        public ExampleMessageList(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IViewComponentResult> InvokeAsync() {
            var model = await mediator.Send(new GetIndex(new Uri("divertedmessages", UriKind.Relative), 10, null));
            return View(model);
        }
    }
}