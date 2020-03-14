namespace VS.Core.PayOn {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Caching;
    using VS.Core.Caching;
    using VS.Core.Payment;

    public class CacheCheckoutDecorator : IRequestHandler<CheckoutRequest, CheckoutResponse> {
        private readonly IRequestHandler<CheckoutRequest, CheckoutResponse> wrappedHandler;
        private readonly IMediator mediator;

        public CacheCheckoutDecorator(IRequestHandler<CheckoutRequest, CheckoutResponse> wrappedHandler, IMediator mediator) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancellationToken) {

            var response = await mediator.Send(new Retrieve<CheckoutResponse>(request.OrderId), cancellationToken);

            if (response is object) {
                var status = await mediator.Send(new CheckoutStatusRequest(request.OrderId, response.ProviderReference), cancellationToken);

                if (status.Success) {

                    return response;
                }                                    
                
            }

            response = await wrappedHandler.Handle(request, cancellationToken);
            var cacheResponse = await mediator.Send(new Cache<CheckoutResponse>(response, request.OrderId, new CacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(29) }), cancellationToken);
            return cacheResponse;
        }
    }
}
