namespace VS.Core.Storage.Handlers {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Storage;

    public class RemoveHandler : IRequestHandler<Remove, bool> {

        private readonly IStorageClient storageClient;

        public RemoveHandler(IStorageClient storageClient) {            
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        }

        public Task<bool> Handle(Remove request, CancellationToken cancellationToken) {
            return storageClient.Remove(request.Location, cancellationToken);
        }
    }
}
