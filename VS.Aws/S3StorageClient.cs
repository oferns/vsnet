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
    using System.Linq;
    using Amazon.S3.Model;
    using System.Net;

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

        public async Task<bool> Exists(Uri uri, CancellationToken cancel) {
            var bucket = BucketNameFromUri(uri);
            if (!await s3.DoesS3BucketExistAsync(bucket)) {
                return false;
            }

            // Is it a bucket?
            if (uri.GetComponents(UriComponents.Path, UriFormat.Unescaped).EndsWith(bucket)) {
                return true;
            }

            // Get the response
            var response = await s3.GetAllObjectKeysAsync(bucket, S3KeyFromUri(uri), null);

            // Is it a file?
            return response.Count() == 1;
        }

        public async Task<Stream> Get(Uri uri, CancellationToken cancel) {
            return await s3.GetObjectStreamAsync(BucketNameFromUri(uri), uri.LocalPath, null, cancel);
        }

        public async Task<PagedIndex> Index(Uri uri, int pageSize, string token, CancellationToken cancel) {
            var bucket = BucketNameFromUri(uri);
            var prefix = S3KeyFromUri(uri);

            var request = new ListObjectsV2Request {
                BucketName = bucket,
                ContinuationToken = token,
                MaxKeys = pageSize,
                Prefix = prefix
            };

            var response = await s3.ListObjectsV2Async(request, cancel);

            var items = response.S3Objects.Select(s => new IndexItem(new Uri(this.BaseUri, Path.Combine(s.BucketName, s.Key)), default(ContentDisposition), default(ContentType)));
            return new PagedIndex(items, pageSize, response.ContinuationToken, response.NextContinuationToken, token);
        }

        public async Task<Uri> Put(Stream stream, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel) {
            var bucket = BucketNameFromUri(uri);

            try {
                await s3.EnsureBucketExistsAsync(bucket);   // OJF: I dont know why this throws. The error is "Your previous request to create the named bucket succeeded and you already own it"
            } catch (AmazonS3Exception ex) {
                if (!ex.StatusCode.Equals(HttpStatusCode.Conflict)) {
                    throw;
                }
            }
            var key = S3KeyFromUri(uri);

            var request = new PutObjectRequest {
                BucketName = bucket,
                Key = key,
                ContentType = contentType.ToString(),
                InputStream = stream,
                AutoCloseStream = false,
                AutoResetStreamPosition = true
            };

            request.Headers.ContentDisposition = contentDisposition.ToString();

            _ = await s3.PutObjectAsync(request, cancel);

            return uri;
        }

        public async Task<bool> Remove(Uri uri, CancellationToken cancel) {
            if (!await Exists(uri, cancel)) {
                // Raise warning?
                return false;
            }

            var bucket = BucketNameFromUri(uri);
            var key = S3KeyFromUri(uri);

            await s3.DeleteAsync(bucket, key, null, cancel);
            return true;
        }

        public Task<Uri> TemporaryLink(AccessLevel accessLevel, Uri uri, DateTime start, DateTime expiry, CancellationToken cancel) {

            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            var bucket = BucketNameFromUri(uri);
            var key = S3KeyFromUri(uri);

            var request = new GetPreSignedUrlRequest {
                BucketName = bucket,
                Key = key,
                Expires = expiry,
                Verb = HttpVerb.GET // TODO: Tie up properly to access level
            };

            var response = s3.GetPreSignedURL(request);

            return Task.FromResult(new Uri(this.BaseUri, response));
        }

        private string BucketNameFromUri(Uri uri) {
            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            var path = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) {
                throw new FormatException("Cannot get bucket name from uri path");
            }
            return parts[0];
        }


        private string S3KeyFromUri(Uri uri) {
            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            var path = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length <= 1) {
                return string.Empty;
            }

            return string.Join('/', parts.Skip(1));
        }
    }
}
