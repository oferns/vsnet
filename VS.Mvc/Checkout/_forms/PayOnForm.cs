namespace VS.Mvc.Checkout._forms {
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Routing;
    using VD.PayOn;
    using VS.Abstractions.Data.Filtering;
    using VS.Core.Data;
    using VS.Core.Payment;
    using VS.Data.PostGres.Book;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "PayOnForm")]
    public class PayOnForm : LocalizedViewComponent {

        private readonly IMediator mediator;
        private readonly PayOnOptions options;
        private readonly IActionContextAccessor actionContext;
        private readonly LinkGenerator link;

        public PayOnForm(IMediator mediator, PayOnOptions options, IActionContextAccessor actionContext, LinkGenerator link) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.actionContext = actionContext ?? throw new ArgumentNullException(nameof(actionContext));
            this.link = link ?? throw new ArgumentNullException(nameof(link));
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            if (!long.TryParse(actionContext.ActionContext.RouteData.Values["orderId"]?.ToString() ?? string.Empty, out var orderId)) {
                return View();
            }

            var orderFilter = new Filter<VwOrder> { new Clause<VwOrder>("OrderId", EvalOp.Equals, orderId) };

            var order = await mediator.Send(new GetOne<VwOrder>(orderFilter));

            if (order is null) {
                return View();
            }


            var response = await mediator.Send(new CheckoutRequest(100, "GBP", order.OrderId.ToString(), order.OrderId.ToString()));

            if (!response.Success) {

            }

            var returnUri = new Uri(link.GetUriByAction(ViewContext.HttpContext, "Callback", "Checkout", new { orderId = order.OrderId }));

            var model = new PayOnFormViewModel {
                OrderId = orderId,
                OppWaId = response.ProviderReference,
                CardProviders = new[] { "Visa", "MasterCard" },
                CardFormAction = new Uri(options.BaseUri, $"/v1/checkouts/{response.ProviderReference}"),
                FormAction = new Uri(options.BaseUri, $"/v1/checkouts/{response.ProviderReference}/payment"),
                ReturnUrl = returnUri
            };

            return View(model);
        }
    }
}