
namespace VS.Core.Caching {

    using MediatR;

    public class Retrieve<T> : IRequest<T> {


        public Retrieve(string key) {
            Key = key;
        }

        public string Key { get; }
    }
}
