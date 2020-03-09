
namespace VS.Core.Payment.Handlers {
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions;

    public class CheckoutStatusHandler : IRequestHandler<CheckoutStatusRequest, CheckoutStatusResponse> {
        private readonly IContext context;

        public CheckoutStatusHandler(IContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public Task<CheckoutStatusResponse> Handle(CheckoutStatusRequest request, CancellationToken cancellationToken) {
              //((ClaimsIdentity)context.User.Identity).AddClaim(new Claim())
            
            
            return Task.FromResult<CheckoutStatusResponse>(default);
        }
    }
}
