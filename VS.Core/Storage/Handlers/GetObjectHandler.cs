
namespace VS.Core.Storage.Handlers {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Storage;

    public class GetObjectHandler<T> : IRequestHandler<GetObject<T>, T> where T : class {

        private readonly IStorageClient storageClient;

        public GetObjectHandler(IStorageClient storageClient) {
            
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        }

        public Task<T> Handle(GetObject<T> request, CancellationToken cancellationToken) {
            return storageClient.GetObject<T>(request.Path, cancellationToken);
        }
    }
}
