namespace VS.Core.Storage {
    using MediatR;
    using System;

    public class Remove : IRequest<bool> {

        public Remove(Uri location) {
            Location = location;
        }

        public Uri Location { get; private set; }

    }
}