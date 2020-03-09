namespace VS.Core.Storage {

    using System;
    using MediatR;

    public class GetObject<T> : IRequest<T> where T: class  {

        public GetObject(Uri path) {
            Path = path;
        }

        public Uri Path { get; private set; }
    }
}