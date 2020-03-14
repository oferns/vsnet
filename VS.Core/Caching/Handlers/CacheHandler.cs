namespace VS.Core.Caching.Handlers {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Caching;

    public class CacheHandler<T> : IRequestHandler<Cache<T>, T> {
        private readonly ICacheClient<T> client;

        public CacheHandler(ICacheClient<T> client) {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task<T> Handle(Cache<T> request, CancellationToken cancellationToken) {            
            return client.Cache(request.Key, request.Object, request.EntryOptions, cancellationToken).AsTask();
        }
    }
}
