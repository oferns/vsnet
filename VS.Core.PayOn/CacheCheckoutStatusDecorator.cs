namespace VS.Core.PayOn {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Caching;
    using VS.Core.Caching;
    using VS.Core.Payment;

    public class CacheCheckoutStatusDecorator : IRequestHandler<CheckoutStatusRequest, CheckoutStatusResponse> {
        private readonly IRequestHandler<CheckoutStatusRequest, CheckoutStatusResponse> wrappedHandler;
        private readonly IMediator mediator;

        public CacheCheckoutStatusDecorator(IRequestHandler<CheckoutStatusRequest, CheckoutStatusResponse> wrappedHandler, IMediator mediator) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<CheckoutStatusResponse> Handle(CheckoutStatusRequest request, CancellationToken cancellationToken) {

            if (!request.Refresh) {
                var response = await mediator.Send(new Retrieve<CheckoutStatusResponse>(request.OrderId), cancellationToken);

                if (response is object) {
                    return response;
                }
            }

            var wrappedResponse = await wrappedHandler.Handle(request, cancellationToken);
            var cacheResponse = await mediator.Send(new Cache<CheckoutStatusResponse>(
                    wrappedResponse, 
                    request.OrderId, 
                    new CacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(31) }), 
                    cancellationToken);

            return cacheResponse;
        }
    }
}
