namespace VS.Abstractions.Storage {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Storage.Paging;

    public interface IStorageClient {


        Uri BaseUri { get; }

        Task<PagedIndex> Index(Uri prefix, int pageSize, string token, CancellationToken cancel);

        Task<Stream> Get(Uri uri, CancellationToken cancel);

        Task<Uri> Put(Stream stream, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel);

        Task<bool> Exists(Uri uri, CancellationToken cancel);

        Task<bool> Remove(Uri uri, CancellationToken cancel);

        Task<Uri> TemporaryLink(AccessLevel accessLevel, Uri path, DateTime start, DateTime expiry, CancellationToken cancel);
    }
}