namespace VS.Core.Storage {
    using MediatR;
    using System;
    using System.IO;
    using System.Net.Mime;

    public class Put : IRequest<Uri> {

        public Uri RelativePath { get; set; }
        public Stream Stream { get; set; }

        public ContentDisposition ContentDisposition { get; set; }

        public ContentType ContentType { get; set; }
    }
}