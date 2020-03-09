namespace VS.Core.Storage {
    using MediatR;
    using System;
    using System.IO;

    public class Get : IRequest<Stream> {

        public Get(Uri Path) {
            this.Path = Path ?? throw new ArgumentNullException(nameof(Path));
        }

        public Uri Path { get; private set; }
    }
}
