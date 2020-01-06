namespace VS.Core.Storage {
    using MediatR;
    using System;

    public class Remove : IRequest<bool> {

        public Uri Path { get; set; }

    }
}