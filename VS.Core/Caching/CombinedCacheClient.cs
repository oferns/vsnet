namespace VS.Core.Caching {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Caching;

    public class CombinedCacheClient<T> : ICacheClient<T> {
        private readonly ICacheClient<T>[] cacheClients;

        public CombinedCacheClient(ICacheClient<T>[] cacheClients) {
            this.cacheClients = cacheClients ?? throw new ArgumentNullException(nameof(cacheClients));
        }

        public async ValueTask<T> Cache(string key, T obj, CacheEntryOptions options, CancellationToken cancel) {

            foreach (var client in cacheClients) {
                _ = await client.Cache(key, obj, options, cancel);
            }

            return obj;
        }

        public async ValueTask<T> Remove(string key, CancellationToken cancel) {
            foreach (var client in cacheClients) {
                _ = await client.Remove(key, cancel);
            }

            return default;
        }

        public async ValueTask<T> Retrieve(string key, CancellationToken cancel) {


            for (var x = 0; x < cacheClients.Length; x++) {
                var client = cacheClients[x];
                var obj = await client.Retrieve(key, cancel);
                if (obj is object) {
                    if (x > 0) {
                        for (var y = 0; y < x; y++) {
                            await cacheClients[y].Cache(key, obj, default, cancel);
                        }
                    }
                    return obj;
                }
            }

            return default;
        }
    }
}