namespace VS.Core.Storage {
    using MediatR;
    using System;

    public class Exists : IRequest<Uri> {
        public Uri Path { get; set; }
    }
}
