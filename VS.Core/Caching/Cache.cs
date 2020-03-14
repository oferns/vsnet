namespace VS.Core.Caching {

    using MediatR;
    using VS.Abstractions.Caching;

    public class Cache<T> : IRequest<T> {


        public Cache(T obj, string key, CacheEntryOptions entryOptions) {
            Object = obj;
            Key = key;
            EntryOptions = entryOptions;
        }

        public T Object { get; }
        public string Key { get; }
        public CacheEntryOptions EntryOptions { get; }
    }
}
