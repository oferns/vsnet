namespace VS.Mvc.Payment._forms {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using VD.Noire;
    using VS.Abstractions.Data.Filtering;
    using VS.Core.Data;
    using VS.Data.PostGres.Book;
    using VS.Mvc._Extensions;


    [ViewComponent(Name = "NoireStatusForm")]
    public class StatusForm : LocalizedViewComponent {


        private readonly IMediator mediator;
        private readonly INoireClient client;
        private readonly IActionContextAccessor context;

        public StatusForm(IMediator mediator, INoireClient client, IActionContextAccessor context) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var resourcePath = context.ActionContext.RouteData.Values["resourcePath"]?.ToString();
            if (string.IsNullOrEmpty(resourcePath)) {
                return View();
            }

            if (!long.TryParse(context.ActionContext.RouteData.Values["orderId"]?.ToString() ?? string.Empty, out var orderId)) {
                return View();
            }
    

            var orderFilter = new Filter<VwOrder> { new Clause<VwOrder>("OrderId", EvalOp.Equals, orderId) };

            var order = await mediator.Send(new GetOne<VwOrder>(orderFilter));

            if (order is null) {
                return View();
            }


            var model = await client.CheckoutStatus(resourcePath, CancellationToken.None);



            return View(model);

        }


    }
}
