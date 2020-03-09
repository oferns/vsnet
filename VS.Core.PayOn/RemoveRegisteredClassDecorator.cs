namespace VS.Core.PayOn {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VD.PayOn;
    using VS.Core.Payment;
    using VS.Core.Storage;

    public class RemoveRegisteredClassDecorator : IRequestHandler<RemoveRegisteredCardRequest, bool> {
        private readonly IPayOnClient PayOnClient;
        private readonly IMediator mediator;

        public RemoveRegisteredClassDecorator(IRequestHandler<RemoveRegisteredCardRequest, bool> wrappedHandler, IPayOnClient PayOnClient, IMediator mediator) {
            _ = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.PayOnClient = PayOnClient ?? throw new ArgumentNullException(nameof(PayOnClient));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(RemoveRegisteredCardRequest request, CancellationToken cancellationToken) {

            var removedFromProvider = await PayOnClient.DeRegisterCard(new DeRegisterCard(request.ReferenceId), cancellationToken);
            if (removedFromProvider.Result.DidntFail()) {

                var uri = new Uri($"vs-cards/{request.UserId}/{request.ReferenceId}", UriKind.Relative);
                return await mediator.Send(new Remove(uri), cancellationToken);

            }
            return false;
        }
    }
}
