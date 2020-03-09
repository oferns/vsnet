namespace VS.Mvc.Payment {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using VS.Core.Payment;

    [Controller]
    public class CheckoutController {
        private readonly IMediator mediator;
        private readonly IActionContextAccessor context;

        public CheckoutController(IMediator mediator, IActionContextAccessor context) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult Index(long orderId) {
            
            if (orderId.Equals(0)) {
                return new BadRequestResult();
            }

            context.ActionContext.RouteData.Values.Add("orderId", orderId);
            return new ViewResult { ViewName = "Checkout" };
        }


        [HttpGet]
        public async Task<IActionResult> Callback(long orderId, string resourcePath, string id, CancellationToken cancel) {

            if (orderId.Equals(0) || string.IsNullOrEmpty(id)) {
                return new BadRequestResult();
            }

            var paymentStatus = await mediator.Send(new CheckoutStatusRequest(id), cancel);

            if (paymentStatus.Success) {
                new RedirectToActionResult("Complete", "Checkout", new { orderId = orderId });
            }
            
            return new RedirectToActionResult("Pending", "Checkout", new { orderId = orderId });
        }



        [HttpGet]
        public IActionResult Status(long orderId) {

            return new ViewResult { ViewName = "Status" };
        }


    }
}
