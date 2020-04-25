
namespace VS.Redis {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using StackExchange.Redis;
    using VS.Abstractions;
    using VS.Abstractions.Caching;
    using VS.Abstractions.Logging;

    public class RedisCacheClient<T> : ICacheClient<T> {

        private readonly IDatabase db;
        private readonly ISerializer serializer;
        private readonly IContext context;
        private readonly Guid typeId;

        public RedisCacheClient(IDatabase db, ISerializer serializer, IContext context) {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            typeId = typeof(T).GUID;
        }

        public async ValueTask<T> Cache(string key, T obj, CacheEntryOptions options, CancellationToken cancel) {
            context.Log.LogInfo($"Redis Caching {typeof(T).FullName} with key {key} for user {context.User.Identity.Name}.");
            key = $"{typeId}-{key}";
            var val = await serializer.SerializeToByteArray<T>(obj, cancel);
            if (await db.StringSetAsync(key, val, options.AbsoluteExpirationRelativeToNow)) {
                return obj;
            }
            context.Log.LogInfo($"Redis Caching FAILED {typeof(T).FullName} with key {key} for user {context.User.Identity.Name}.");

            return default;
        }
        
        public async ValueTask<T> Remove(string key, CancellationToken cancel) {
            context.Log.LogInfo($"Removing {typeof(T).FullName} from Redis Cache with key {key} for user {context.User.Identity.Name}.");

            key = $"{typeId}-{key}";
            var obj = await Retrieve(key, cancel);
            if (obj is object) {
                context.Log.LogInfo($"Key found in Removal of {typeof(T).FullName} from Redis Cache with key {key} for user {context.User.Identity.Name}.");
                if (await db.KeyDeleteAsync(key)) {
                    context.Log.LogInfo($"Suscces in removal of {typeof(T).FullName} from Redis Cache with key {key} for user {context.User.Identity.Name}.");
                }
                return obj;
            }
            context.Log.LogInfo($"Key NOT found in Removal of {typeof(T).FullName} from Redis Cache with key {key} for user {context.User.Identity.Name}.");
            
            return default;
        }

        public async ValueTask<T> Retrieve(string key, CancellationToken cancel) {
            context.Log.LogInfo($"Retrieving {typeof(T).FullName} from Redis Cache with key {key} for user {context.User.Identity.Name}.");
            key = $"{typeId}-{key}";
            var bytes = await db.StringGetAsync(key);
            if (bytes.HasValue) {
                context.Log.LogInfo($"Key found in Retrieval of {typeof(T).FullName} from Redis Cache with key {key} for user {context.User.Identity.Name}.");
                return await serializer.Deserialize<T>(bytes, cancel);
            }
            
            context.Log.LogInfo($"Key NOT found in Retrieval of {typeof(T).FullName} from Redis Cache with key {key} for user {context.User.Identity.Name}.");
            return default;
        }
    }
}
