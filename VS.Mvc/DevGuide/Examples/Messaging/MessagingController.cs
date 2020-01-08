namespace VS.Mvc.DevGuide.Examples.Messaging {
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VS.Core.Messaging.Queue;
    using VS.Mvc._Extensions;
    
    [Area("DevGuide")]
    [SubArea("Examples")]
    [Controller]
    public class MessagingController {
        private readonly IMediator mediator;

        public MessagingController(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public IActionResult Index() => new ViewResult { ViewName = "Messaging" };


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToQueue(TestMessageModel model) {

            var message = await mediator.Send(new Enqueue<TestMessageModel>(model));

            return new RedirectToActionResult("Index", "Messaging", new { area = "DevGuide", subarea = "Examples" });
        }
    }
}
