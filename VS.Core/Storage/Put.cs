namespace VS.Core.Storage {
    using MediatR;
    using System;
    using System.IO;
    using System.Net.Mime;

    public class Put : IRequest<Uri> {

        public Put(Uri uri, Stream stream, ContentDisposition contentDisposition, ContentType contentType) {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            ContentDisposition = contentDisposition ?? throw new ArgumentNullException(nameof(contentDisposition));
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
        }

        public Uri Uri { get; }

        public Stream Stream { get; }

        public ContentDisposition ContentDisposition { get; }

        public ContentType ContentType { get; }

    }
}