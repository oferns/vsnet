namespace VS.Mvc.Payment {
    using System;
    using System.Dynamic;
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
        public async Task<IActionResult> Callback(string orderId, string resourcePath, string id, CancellationToken cancel) {

            if (orderId.Equals(0) || string.IsNullOrEmpty(id)) {
                return new BadRequestResult();
            }

            var paymentStatus = await mediator.Send(new CheckoutStatusRequest(orderId, id, true), cancel);

            if (paymentStatus.Success) {

                if (paymentStatus.Complete) {
                  return new RedirectToActionResult("Complete", "Checkout", new { id = orderId, @ref = id });
                }

                return new RedirectToActionResult("Pending", "Checkout", new { id = orderId, @ref = id });
            }                                                                            
            
            return new RedirectToActionResult("Failed", "Checkout", new { id = orderId, @ref = id });
        }






        [HttpGet]
        public IActionResult Complete(string id, string @ref) {

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(@ref)) {
                return new BadRequestResult();
            }

            context.ActionContext.RouteData.Values.Add("orderId", id);
            context.ActionContext.RouteData.Values.Add("paymentRef", @ref);

            return new ViewResult { ViewName = "Complete" };
        }


        [HttpGet]
        public IActionResult Pending(string id, string @ref) {

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(@ref)) {
                return new BadRequestResult();
            }

            context.ActionContext.RouteData.Values.Add("orderId", id);
            context.ActionContext.RouteData.Values.Add("paymentRef", @ref);

            return new ViewResult { ViewName = "Pending" };
        }

        [HttpGet]
        public IActionResult Failed(string id, string @ref) {

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(@ref)) {
                return new BadRequestResult();
            }

            context.ActionContext.RouteData.Values.Add("orderId", id);
            context.ActionContext.RouteData.Values.Add("paymentRef", @ref);

            return new ViewResult { ViewName = "Failed" };
        }

    }
}
