namespace VS.Mvc.DevGuide.Examples.MessagingExamples._Lists {
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Core.Storage;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "ExampleMessageList")]
    public class ExampleMessageList : LocalizedViewComponent {
        private readonly IMediator mediator;
        private readonly IContext context;

        public ExampleMessageList(IMediator mediator, IContext context) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IViewComponentResult> InvokeAsync() {
            var model = await mediator.Send(new GetIndex(new Uri($"{context.Host}/divertedmessages", UriKind.Relative), 10, null));
            return View(model);
        }
    }
}