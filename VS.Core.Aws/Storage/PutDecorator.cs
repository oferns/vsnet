namespace VS.Core.Aws.Storage {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.Runtime;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using VS.Abstractions.Storage;
    using VS.Core.Storage;

    public class PutDecorator : IRequestHandler<Put, Uri> {
        private readonly IRequestHandler<Put, Uri> wrappedHandler;
        private readonly IStorageClient storageClient;
        private readonly ILogger<PutDecorator> log;

        public PutDecorator(IRequestHandler<Put, Uri> wrappedHandler, IStorageClient storageClient, ILogger<PutDecorator> log) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<Uri> Handle(Put request, CancellationToken cancel) {

            try {
                return await storageClient.Put(request.Stream, request.Uri, request.ContentDisposition, request.ContentType, cancel);
            } catch (AmazonServiceException serviceException) {
                log.LogError(serviceException, "Oh dear! Trying wrapped handler");
                return await wrappedHandler.Handle(request, cancel);
            }
        }
    }
}