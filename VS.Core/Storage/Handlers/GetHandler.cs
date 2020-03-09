namespace VS.Core.Storage.Handlers {

    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Storage;

    public class GetHandler : IRequestHandler<Get, Stream> {

        private readonly IStorageClient client;

        public GetHandler(IStorageClient client) {
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
        }

        public Task<Stream> Handle(Get request, CancellationToken cancellationToken) {
            return this.client.Get(request.Path, cancellationToken);
        }
    }
}
