namespace VS.Mvc.Payment {
    using System;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [Controller]
    public class PaymentController {
        private readonly IMediator mediator;

        public PaymentController(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public IActionResult Index(Guid? stamp) {

            return new ViewResult { ViewName = "Payment" };
        }



    }
}
