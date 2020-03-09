namespace VS.Core.Storage.Handlers {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Storage;

    public class PutObjectHandler<T> : IRequestHandler<PutObject<T>, T> where T : class {

        private readonly IStorageClient storageClient;

        public PutObjectHandler(IStorageClient storageClient) {

            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        }

        public Task<T> Handle(PutObject<T> request, CancellationToken cancellationToken) {
            return Task.FromResult<T>(default);
        }
    }
}