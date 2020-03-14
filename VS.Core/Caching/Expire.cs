namespace VS.Core.Caching {

    using MediatR;

    public class Expire<T> : IRequest<T> {


        public Expire(string key) {
            Key = key;
        }

        public string Key { get; }

    }
}
