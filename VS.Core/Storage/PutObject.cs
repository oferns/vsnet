namespace VS.Core.Storage {

    using System;
    using MediatR;

    public class PutObject<T> : IRequest<Uri> where T : class {

        public PutObject(T @object, Uri location) {
            Object = @object;
            Location = location;
        }

        public T Object { get; }
        public Uri Location { get; }
    }
}
