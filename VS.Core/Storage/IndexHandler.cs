namespace VS.Core.Storage {

    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Storage;
    using VS.Abstractions.Storage.Paging;
    using VS.Core.Storage;

    public class IndexHandler : IRequestHandler<Index, PagedIndex> {
        private readonly IStorageClient client;

        public IndexHandler(IStorageClient client) {
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
        }

        public async Task<PagedIndex> Handle(Index request, CancellationToken cancel) {
           return await client.Index(request.Prefix, request.PageSize, request.Token, cancel);
        }
    }
}
