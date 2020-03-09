namespace VS.Core.Payment.Handlers {
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions;
    using VS.Core.Identity;
    using VS.Core.Payment;
    using VS.Abstractions.Logging;

    public class CheckoutHandler : IRequestHandler<CheckoutRequest, CheckoutResponse> {
        private readonly IContext context;

        public CheckoutHandler(IContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancel) {
                        
            context.Log.Log($"Checkout {request.Reference} Requested by user {context.User.FindFirstValue(IdClaimTypes.UserIdentifier)}");
            return Task.FromResult<CheckoutResponse>(default);            
        }
    }
}
