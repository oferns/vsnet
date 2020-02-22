namespace VS.Mvc.Payment {
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using VS.Core.Data;
    using VS.Data.PostGres.App;

    [Controller]
    public class PaymentController {
        private readonly IMediator mediator;
        private readonly IActionContextAccessor context;

        public PaymentController(IMediator mediator, IActionContextAccessor context) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult Index(long orderId) {
            
            if (orderId.Equals(0)) {
                return new BadRequestResult();
            }

            context.ActionContext.RouteData.Values.Add("orderId", orderId);
            return new ViewResult { ViewName = "Payment" };
        }


        [HttpGet]
        public IActionResult Status(long orderId, string resourcePath) {

            if (orderId.Equals(0) || string.IsNullOrEmpty(resourcePath)) {
                return new BadRequestResult();
            }

            context.ActionContext.RouteData.Values.Add("orderId", orderId);
            context.ActionContext.RouteData.Values.Add("resourcePath", resourcePath);

            return new ViewResult { ViewName = "Status" };
        }

    }
}
