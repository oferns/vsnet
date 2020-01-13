
namespace VS.Core.Aws.Storage {
    using Amazon.Runtime;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Storage;
    using VS.Abstractions.Storage.Paging;
    using VS.Core.Storage;

    public class GetIndexRequestDecorator : IRequestHandler<GetIndex, PagedIndex> {

        private readonly IRequestHandler<Core.Storage.GetIndex, PagedIndex> wrappedHandler;
        private readonly IStorageClient storageClient;
        private readonly ILogger<GetIndexRequestDecorator> log;

        public GetIndexRequestDecorator(IRequestHandler<GetIndex, PagedIndex> wrappedHandler, IStorageClient storageClient, ILogger<GetIndexRequestDecorator> log) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<PagedIndex> Handle(GetIndex request, CancellationToken cancel) {
            try {
                return await storageClient.Index(request.Prefix, request.PageSize, request.Token, cancel);
            } catch (AmazonServiceException serviceException) {
                log.LogError(serviceException, "Oh dear! Trying wrapped handler");
                return await wrappedHandler.Handle(request, cancel);
            }
        }
    }
}
