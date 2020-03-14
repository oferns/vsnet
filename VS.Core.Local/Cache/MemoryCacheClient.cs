
namespace VS.Core.Local.Cache {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using VS.Abstractions;
    using VS.Abstractions.Caching;
    using VS.Abstractions.Logging;

    public class MemoryCacheClient<T> : ICacheClient<T> {
        private readonly IMemoryCache memoryCache;
        private readonly IContext context;
        private readonly Guid typeId;

        public MemoryCacheClient(IMemoryCache memoryCache, IContext context) {
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            typeId = typeof(T).GUID;
        }

        public ValueTask<T> Cache(string key, T obj, CacheEntryOptions options, CancellationToken cancel) {

            context.Log.LogInfo($"Memory Caching {typeof(T).FullName} with key {key} for user {context.User.Identity.Name}.");
            key = $"{typeId}-{key}";
            return new ValueTask<T>(memoryCache.Set<T>(key, obj, new MemoryCacheEntryOptions { 
                AbsoluteExpiration = options.AbsoluteExpiration, 
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow, 
                SlidingExpiration = options.SlidingExpiration                
            }));
        }

        public ValueTask<T> Remove(string key, CancellationToken cancel) {
            context.Log.LogInfo($"Removing {typeof(T).FullName} from MemoryCache with key {key} for user {context.User.Identity.Name}.");
            key = $"{typeId}-{key}";                        
            if (memoryCache.TryGetValue<T>(key, out var obj)) {
                context.Log.LogInfo($"Key found in Removal of {typeof(T).FullName} from MemoryCache with key {key} for user {context.User.Identity.Name}.");
                memoryCache.Remove(key);
                return new ValueTask<T>(obj);
            }
            context.Log.LogInfo($"Key NOT found in Removal of {typeof(T).FullName} from MemoryCache with key {key} for user {context.User.Identity.Name}.");
            return new ValueTask<T>(default(T));
        }

        public ValueTask<T> Retrieve(string key, CancellationToken cancel) {
            context.Log.LogInfo($"Retrieving {typeof(T).FullName} from MemoryCache with key {key} for user {context.User.Identity.Name}.");
            key = $"{typeId}-{key}";
            var obj = memoryCache.Get<T>(key);

            if (obj is object) {
                context.Log.LogInfo($"Key found in Retrieval of {typeof(T).FullName} from MemoryCache with key {key} for user {context.User.Identity.Name}.");
            } else {
                context.Log.LogInfo($"Key NOT found in Retrieval of {typeof(T).FullName} from MemoryCache with key {key} for user {context.User.Identity.Name}.");
            }

            return new ValueTask<T>(obj);
        }
    }
}
