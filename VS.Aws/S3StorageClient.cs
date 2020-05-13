namespace VS.Aws {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.S3;
    using Amazon.S3.Model;
    using VS.Abstractions;
    using VS.Abstractions.Storage;
    using VS.Abstractions.Storage.Paging;

    public class S3StorageClient : IStorageClient {
        private readonly IAmazonS3 s3;
        private readonly ISerializer serializer;
        private Uri baseUri;

        public S3StorageClient(IAmazonS3 s3, ISerializer serializer) {
            this.s3 = s3 ?? throw new ArgumentNullException(nameof(s3));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public Uri BaseUri {
            get {
                return this.baseUri ?? (baseUri = new Uri(s3.Config.DetermineServiceURL()));
            }
        }

        public async Task<Uri> CreateContainer(AccessLevel accessLevel, string containerName, CancellationToken cancel) {
            await s3.EnsureBucketExistsAsync(containerName);
            return new Uri(this.BaseUri, containerName);
        }

        public async Task<bool> Exists(Uri uri, CancellationToken cancel) {
            var bucket = BucketNameFromUri(uri);
            if (!await s3.DoesS3BucketExistAsync(bucket)) {
                return false;
            }

            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            // Is it a bucket?
            if (uri.GetComponents(UriComponents.Path, UriFormat.Unescaped).EndsWith(bucket)) {
                return true;
            }

            // Get the response
            var response = await s3.GetAllObjectKeysAsync(bucket, S3KeyFromUri(uri), null);

            // Is it a file?
            return response.Count == 1;
        }

        public Task<Stream> Get(Uri uri, CancellationToken cancel) {
            return s3.GetObjectStreamAsync(BucketNameFromUri(uri), S3KeyFromUri(uri), null, cancel);
        }

        public async Task<T> GetObject<T>(Uri uri, CancellationToken cancel) where T : class {
            using var objStream = await Get(uri, cancel);
            return await this.serializer.Deserialize<T>(objStream, cancel).AsTask();
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

            var items = new List<IndexItem>();
            
            foreach (var obj in response.S3Objects) {
                items.Add(new IndexItem(new Uri(this.BaseUri, Path.Combine(obj.BucketName, obj.Key)), default(ContentDisposition), default(ContentType)));
            }
            
            return new PagedIndex(items, pageSize, response.ContinuationToken, response.NextContinuationToken, token);
        }

        public async Task<Uri> Put(Stream stream, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel) {
            var bucket = BucketNameFromUri(uri);

            try {
                await s3.EnsureBucketExistsAsync(bucket);   // OJF: I dont know why this throws. The error is "Your previous request to create the named bucket succeeded and you already own it"
            } catch (AmazonS3Exception ex) {
                if (!(ex.StatusCode.Equals(HttpStatusCode.Conflict) && 
                    ex.Message.Equals("Your previous request to create the named bucket succeeded and you already own it."))) {
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

        public async Task<Uri> PutObject<T>(T @object, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel) where T : class {
            using var stream = new MemoryStream();
            await serializer.SerializeToStream(@object, stream, cancel);
            stream.Seek(0, SeekOrigin.Begin);
            return await Put(stream, uri, contentDisposition, contentType, cancel);
        }

        public async Task<bool> Remove(Uri uri, CancellationToken cancel) {
            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            if (!await Exists(uri, cancel)) {
                // Raise warning?
                return false;
            }

            var bucket = BucketNameFromUri(uri);
            var key = S3KeyFromUri(uri);

            await s3.DeleteAsync(bucket, key, null, cancel);
            return true;
        }

        public Task<Uri> TemporaryLink(AccessLevel accessLevel, Uri uri, DateTimeOffset start, DateTimeOffset expiry, CancellationToken cancel) {

            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            var bucket = BucketNameFromUri(uri);
            var key = S3KeyFromUri(uri);

            var request = new GetPreSignedUrlRequest {
                BucketName = bucket,
                Key = key,
                Expires = expiry.LocalDateTime,
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

            var trimmedParts = new string[parts.Length - 1];
            Array.Copy(parts, 1, trimmedParts, 0, trimmedParts.Length);                  
            return string.Join('/', trimmedParts);
        }        
    }
}
