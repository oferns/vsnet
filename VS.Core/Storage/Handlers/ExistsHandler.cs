namespace VS.Core.Storage.Handlers {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Storage;

    public class ExistsHandler : IRequestHandler<Exists, Uri> {

        private readonly IStorageClient storageClient;

        public ExistsHandler(IStorageClient storageClient) {
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        }

        public async Task<Uri> Handle(Exists request, CancellationToken cancellationToken) {
            if (await storageClient.Exists(request.Path, cancellationToken)) {
                return request.Path;
            }
            return default;
        }
    }
}
