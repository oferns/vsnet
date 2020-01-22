namespace VS.Mvc.DevGuide.Examples.Messaging {
    
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using VS.Core.Messaging.Queue;
    using VS.Mvc._Extensions;

    [Area("DevGuide")]
    [SubArea("Examples")]
    [Controller]
    public class MessagingExamplesController {
        private readonly IMediator mediator;

        public MessagingExamplesController(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public IActionResult Index() => new ViewResult { ViewName = "MessagingExamples" };


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToQueue(TestMessageModel model) {

            var message = await mediator.Send(new Enqueue<TestMessageModel>(model));

            return new RedirectToActionResult("Index", "MessagingExamples", new { area = "DevGuide", subarea = "Examples" });
        }
    }
}
