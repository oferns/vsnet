namespace VS.Core.Storage {
    using MediatR;
    using System;
    using System.IO;

    public class Get : IRequest<Stream> {

        public Uri Path { get; set; }
    }
}
