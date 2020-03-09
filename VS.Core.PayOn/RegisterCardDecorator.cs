namespace VS.Core.PayOn {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VD.PayOn;
    using VS.Core.Payment;


    public class RegisterCardDecorator : IRequestHandler<RegisterCardRequest, ReqisterCardResponse> {

        private readonly IPayOnClient client;

        public RegisterCardDecorator(IRequestHandler<RegisterCardRequest, ReqisterCardResponse> wrappedHandler, IPayOnClient client) {
            _ = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task<ReqisterCardResponse> Handle(RegisterCardRequest request, CancellationToken cancellationToken) {
            var PayOnResponse = await client.RegisterCard(new RegisterCard(), cancellationToken);            
            return new ReqisterCardResponse(PayOnResponse.Result.DidntFail(), PayOnResponse.Id, PayOnResponse.Timestamp);
        }
    }
}
