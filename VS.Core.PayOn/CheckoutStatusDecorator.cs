
namespace VS.Core.PayOn {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VD.PayOn;
    using VS.Abstractions;
    using VS.Core.Payment;


    public class CheckoutStatusDecorator : IRequestHandler<CheckoutStatusRequest, CheckoutStatusResponse> {
        private readonly IRequestHandler<CheckoutStatusRequest, CheckoutStatusResponse> wrappedHandler;
        private readonly IPayOnClient client;
        private readonly IContext context;

        public CheckoutStatusDecorator(IRequestHandler<CheckoutStatusRequest, CheckoutStatusResponse> wrappedHandler, IPayOnClient client, IContext context) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<CheckoutStatusResponse> Handle(CheckoutStatusRequest request, CancellationToken cancellationToken) {

            _ = await wrappedHandler.Handle(request, cancellationToken);

            var payOnResponse = await client.CheckoutStatus(request.ProviderId, cancellationToken); ;

            var obj = new CheckoutStatusResponse(payOnResponse.Result.DidntFail(), payOnResponse.Id, payOnResponse.Timestamp, payOnResponse.Result.IsSuccess());

            return obj;
        }
    }
}
