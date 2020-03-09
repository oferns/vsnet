namespace VS.Core.Storage {

    using System;
    using MediatR;
    using VS.Abstractions.Storage;

    public class TempLink : IRequest<Uri> {
        public TempLink(AccessLevel accessLevel, Uri path, DateTimeOffset starts, DateTimeOffset expires) {
            AccessLevel = accessLevel;
            Path = path;
            Starts = starts;
            Expires = expires;
        }

        public AccessLevel AccessLevel { get; set; }

        public Uri Path { get; set; }
        public DateTimeOffset Starts { get; }
        public DateTimeOffset Expires { get; }
   

    }
}
