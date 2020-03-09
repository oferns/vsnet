namespace VS.Core.Storage.Handlers {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Storage;

    public class PutHandler : IRequestHandler<Put, Uri> {

        private readonly IMediator mediator;
        private readonly IStorageClient storageClient;

        public PutHandler(IMediator mediator, IStorageClient storageClient) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        }

        public async Task<Uri> Handle(Put request, CancellationToken cancel) {
            var uri = await storageClient.Put(request.Stream, request.Uri, request.ContentDisposition, request.ContentType, cancel);
            await mediator.Publish(new ChangeNotification<Uri>(request, uri), cancel);
            return uri;
        }
    }
}