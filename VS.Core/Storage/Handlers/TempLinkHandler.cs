namespace VS.Core.Storage.Handlers {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Storage;

    public class TempLinkHandler : IRequestHandler<TempLink, Uri> {

        private readonly IStorageClient storageClient;

        public TempLinkHandler(IStorageClient storageClient) {
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        }

        public Task<Uri> Handle(TempLink request, CancellationToken cancellationToken) {
            return storageClient.TemporaryLink(request.AccessLevel, request.Path, request.Starts, request.Expires, cancellationToken);
        }
    }
}