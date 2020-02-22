namespace VS.Mvc.Payment._forms {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using VD.Noire;
    using VS.Abstractions;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Payment;
    using VS.Core.Data;
    using VS.Core.Payment;
    using VS.Data.PostGres.App;
    using VS.Data.PostGres.Book;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "NoireForm")]
    public class NoireForm : LocalizedViewComponent {

        private readonly IMediator mediator;
        private readonly NoireOptions options;
        private readonly IActionContextAccessor context;

        public NoireForm(IMediator mediator, NoireOptions options, IActionContextAccessor context) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            if (!long.TryParse(context.ActionContext.RouteData.Values["orderId"]?.ToString() ?? string.Empty, out var orderId)) {
                return View();
            }
            
       
            var orderFilter = new Filter<VwOrder> { new Clause<VwOrder>("OrderId", EvalOp.Equals, orderId) };

            var order = await mediator.Send(new GetOne<VwOrder>(orderFilter));

            if (order is null) {
                return View();
            }

            var response = await mediator.Send(new StartCheckoutRequest(100, "GBP", order.OrderId.ToString(), order.OrderId.ToString()));

            if (!response.Success) { 
            
            }
            
            var model = new NoireFormViewModel {
                OrderId = orderId,
                CardProviders = new[] { "Visa", "MasterCard" },
                FormAction = new Uri($"https://test.oppwa.com/v1/checkouts/{response.ProviderReference}/payment"),
                ReturnUrl = new Uri($"http://127.0.0.1:5000/payment/status?orderId={orderId}")
            };

            return View(model);
        }
    }
}