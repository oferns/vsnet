namespace VS.Abstractions.Caching {

    using System.Threading.Tasks;

    public interface ICacheManager {

        ValueTask Flush();
    }
}
