namespace VS.Abstractions.Caching {
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICacheClient<T> {

        ValueTask<T> Retrieve(string key, CancellationToken cancel);
        ValueTask<T> Cache(string key, T obj, CacheEntryOptions options, CancellationToken cancel);
        ValueTask<T> Remove(string key, CancellationToken cancel);

    }


}
