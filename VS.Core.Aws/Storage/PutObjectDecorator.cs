namespace VS.Core.Aws.Storage {

    using MediatR;
    using System;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Storage;
    using VS.Core.Storage;

    public class PutObjectDecorator<T> : IRequestHandler<PutObject<T>, Uri> where T : class {
        private readonly IRequestHandler<PutObject<T>, Uri> wrappedHandler;
        private readonly IStorageClient storageClient;

        public PutObjectDecorator(IRequestHandler<PutObject<T>, Uri> wrappedHandler, IStorageClient storageClient) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));            
        }

        public Task<Uri> Handle(PutObject<T> request, CancellationToken cancellationToken) {
            var contentDisposition = new ContentDisposition();
            var contentType = new ContentType("application/json");
            try {
                return storageClient.PutObject(request.Object, request.Location, contentDisposition, contentType, cancellationToken);
            } catch (Exception ex) {
                // TODO: This might be gone
                return this.wrappedHandler.Handle(request, cancellationToken);
            
            }
        }
    }
}