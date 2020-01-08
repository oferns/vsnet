namespace VS.Aws {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Storage;
    using VS.Abstractions.Storage.Paging;
    using Amazon.S3;
    using Amazon.Runtime.SharedInterfaces;

    public class S3StorageClient : IStorageClient {
        private readonly IAmazonS3 s3;
        private Uri baseUri;

        public S3StorageClient(IAmazonS3 s3) {
            this.s3 = s3 ?? throw new ArgumentNullException(nameof(s3));

        }

        public Uri BaseUri {
            get {
                return this.baseUri ?? (baseUri = new Uri(s3.Config.DetermineServiceURL()));
            }
        }

        public Task<bool> Exists(Uri uri, CancellationToken cancel) {
            throw new NotImplementedException();
        }

        public async Task<Stream> Get(Uri uri, CancellationToken cancel) {
            return await s3.GetObjectStreamAsync(BucketNameFromUri(uri), uri.LocalPath, null, cancel);
        }

        public Task<PagedIndex> Index(Uri prefix, int pageSize, string token, CancellationToken cancel) {
            throw new NotImplementedException();
        }

        public Task<Uri> Put(Stream stream, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel) {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(Uri uri, CancellationToken cancel) {
            throw new NotImplementedException();
        }

        public Task<Uri> TemporaryLink(AccessLevel accessLevel, Uri path, DateTime start, DateTime expiry, CancellationToken cancel) {
            throw new NotImplementedException();
        }

        private string BucketNameFromUri(Uri uri) {
            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            var parts = uri.PathAndQuery.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) {
                throw new FormatException("Cannot get bucket name from uri path");
            }
            return parts[0];

        }
    }
}
