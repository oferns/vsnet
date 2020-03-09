namespace VS.Core.PayOn {

    using System;
    using System.IO;
    using System.Net.Mime;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VD.PayOn;
    using VS.Abstractions;
    using VS.Abstractions.Storage;
    using VS.Core.Identity;
    using VS.Core.Payment;


    public class RegisterCardStatusDecorator : IRequestHandler<RegisterCardStatusRequest, RegisterCardStatusResponse> {
        private readonly IRequestHandler<RegisterCardStatusRequest, RegisterCardStatusResponse> wrappedHandler;
        private readonly IPayOnClient client;
        private readonly IContext context;
        private readonly IStorageClient storageClient;
        
        public RegisterCardStatusDecorator(
                        IRequestHandler<RegisterCardStatusRequest, RegisterCardStatusResponse> wrappedHandler,
                        IPayOnClient client,
                        IContext context,
                        IStorageClient storageClient) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
            
        }

        public async Task<RegisterCardStatusResponse> Handle(RegisterCardStatusRequest request, CancellationToken cancellationToken) {

            var PayOnResponse = await client.RegisterCardStatus(request.RegistrationId, cancellationToken);

            var userId = context.User.FindFirst(IdClaimTypes.UserIdentifier);

            if (PayOnResponse.Result.DidntFail()) {

                var contentDisposition = new ContentDisposition {
                    CreationDate = DateTime.Now,
                    FileName = $"{PayOnResponse.Brand}-{PayOnResponse.Card.Last4Digits}.json",
                    Inline = false
                };

                var contentType = new ContentType { MediaType = "application/x-msgpack" };
       

                var path = $"vs-cards/{userId.Value}/{PayOnResponse.Id}";
                var card = new RegisteredCard(PayOnResponse.Card.Holder, PayOnResponse.Card.Last4Digits, PayOnResponse.Brand, $"{PayOnResponse.Card.ExpiryMonth}/{PayOnResponse.Card.ExpiryYear}", PayOnResponse.Id);
                var location = await storageClient.PutObject(card, new Uri(path, UriKind.Relative), contentDisposition, contentType, cancellationToken);
                return new RegisterCardStatusResponse(true, card, PayOnResponse.Id, location, PayOnResponse.Timestamp);
            }

            var obj = new RegisterCardStatusResponse(false, default,  PayOnResponse.Id, default, PayOnResponse.Timestamp);

            return obj;

        }
    }
}
