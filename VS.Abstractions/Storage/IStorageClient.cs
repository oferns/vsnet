namespace VS.Abstractions.Storage {

    using System;
    using System.IO;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Storage.Paging;

    public interface IStorageClient {

        Uri BaseUri { get; }

        Task<PagedIndex> Index(Uri prefix, int pageSize, string token, CancellationToken cancel);

        Task<Stream> Get(Uri uri, CancellationToken cancel);

        Task<T> GetObject<T>(Uri uri, CancellationToken cancel) where T : class;

        Task<Uri> Put(Stream stream, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel);

        Task<Uri> PutObject<T>(T @object, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel) where T : class;

        Task<bool> Exists(Uri uri, CancellationToken cancel);

        Task<bool> Remove(Uri uri, CancellationToken cancel);

        Task<Uri> TemporaryLink(AccessLevel accessLevel, Uri uri, DateTimeOffset start, DateTimeOffset expiry, CancellationToken cancel);

        Task<Uri> CreateContainer(AccessLevel accessLevel, string containerName, CancellationToken cancel);
    }
}