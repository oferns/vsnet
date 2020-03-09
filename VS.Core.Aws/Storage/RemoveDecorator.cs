
namespace VS.Core.Aws.Storage {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Storage;
    using VS.Core.Storage;

    public class RemoveDecorator : IRequestHandler<Remove, bool> {
        private readonly IStorageClient storageClient;

        public RemoveDecorator(IRequestHandler<Remove, bool> wrappedHandler, IStorageClient storageClient) {
            _ = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        }

        public Task<bool> Handle(Remove request, CancellationToken cancellationToken) {
            return storageClient.Remove(request.Location, cancellationToken);
        }
    }
}
