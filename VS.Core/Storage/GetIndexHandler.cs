namespace VS.Core.Storage {

    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Storage;
    using VS.Abstractions.Storage.Paging;
    using VS.Core.Storage;

    public class GetIndexHandler : IRequestHandler<GetIndex, PagedIndex> {
        private readonly IStorageClient client;

        public GetIndexHandler(IStorageClient client) {
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
        }

        public async Task<PagedIndex> Handle(GetIndex request, CancellationToken cancel) {
           return await client.Index(request.Prefix, request.PageSize, request.Token, cancel);
        }
    }
}
