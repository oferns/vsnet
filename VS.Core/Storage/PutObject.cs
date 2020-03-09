namespace VS.Core.Storage {

    using System;
    using MediatR;

    public class PutObject<T> : IRequest<T> {

        public PutObject(T @object, Uri location) {
            Object = @object;
            Location = location;
        }

        public T Object { get; }
        public Uri Location { get; }
    }
}
