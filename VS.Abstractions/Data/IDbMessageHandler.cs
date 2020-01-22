
namespace VS.Abstractions.Data {
    using System.Threading.Tasks;

    public interface IDbMessageHandler<T> where T : class {

        Task MessageRecieved(object sender, T args);
    }
}
