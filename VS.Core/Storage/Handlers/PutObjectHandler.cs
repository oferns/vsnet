namespace VS.Core.Storage.Handlers {

    using System;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Storage;

    public class PutObjectHandler<T> : IRequestHandler<PutObject<T>, Uri> where T : class {

        private readonly IStorageClient storageClient;

        public PutObjectHandler(IStorageClient storageClient) {
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        }

        public Task<Uri> Handle(PutObject<T> request, CancellationToken cancellationToken) {
            var contentDisposition = new ContentDisposition();
            var contentType = new ContentType("application/json");
            return storageClient.PutObject(request.Object, request.Location, contentDisposition, contentType, cancellationToken);
        }
    }
}