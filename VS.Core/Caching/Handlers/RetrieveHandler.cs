
namespace VS.Core.Caching.Handlers {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Caching;

    public class RetrieveHandler<T> : IRequestHandler<Retrieve<T>, T> {

        private readonly ICacheClient<T> client;

        public RetrieveHandler(ICacheClient<T> client) {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public Task<T> Handle(Retrieve<T> request, CancellationToken cancellationToken) {
            return client.Retrieve(request.Key, cancellationToken).AsTask();
        }
    }
}
