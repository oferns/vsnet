namespace VS.Azure {
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Storage;
    using VS.Abstractions.Storage.Paging;

    public class BlobStorageClient : IStorageClient {
        
        private readonly CloudBlobClient client;

        private OperationContext context;
        private BlobRequestOptions Options => new BlobRequestOptions { };

        public BlobStorageClient(CloudBlobClient client) {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        private OperationContext Context => context ?? (context = new OperationContext { LogLevel = (LogLevel)1 });

        public Uri BaseUri => client.BaseUri;

        public async Task<bool> Exists(Uri uri, CancellationToken cancel) {
            try {
                var blobRef = await client.GetBlobReferenceFromServerAsync(new StorageUri(uri), null, null, null, cancel);
                return await blobRef.ExistsAsync();
            } catch (StorageException) {
                return false;
            }
        }

        public async Task<Stream> Get(Uri uri, CancellationToken cancel) {
            if (!await Exists(uri, cancel)) {
                return default(MemoryStream);
            }
            var blobRef = await client.GetBlobReferenceFromServerAsync(uri);
            var stream = new MemoryStream();
            await blobRef.DownloadToStreamAsync(stream);
            stream.Position = 0; // OJF: Is this needed?
            return stream;
        }

        public async Task<PagedIndex> Index(Uri prefix, int pageSize, string token, CancellationToken cancel) {
            var container = ContainerNameFromUri(prefix);

            var containerRef = client.GetContainerReference(container);

            var ctoken = default(BlobContinuationToken);

            if (!string.IsNullOrWhiteSpace(token)) {
                ctoken = new BlobContinuationToken {
                    NextMarker = token,
                    TargetLocation = StorageLocation.Primary
                };
            } else {

                if (!await containerRef.ExistsAsync(Options, context, cancel)) {
                    return new PagedIndex(Enumerable.Empty<IndexItem>(), pageSize, null);
                }
            }

            var blobName = BlobNameFromUri(prefix);
            var resultsSegment = await containerRef.ListBlobsSegmentedAsync(blobName, true, BlobListingDetails.Metadata, pageSize, ctoken, Options, Context, cancel);

            var blobs = resultsSegment.Results.Select(r => {
                var blob = (CloudBlockBlob)r;
                return new IndexItem(r.Uri, new ContentDisposition(blob.Properties.ContentDisposition), new ContentType(blob.Properties.ContentType));
            });

            return new PagedIndex(blobs, pageSize, token, resultsSegment.ContinuationToken?.NextMarker);
        }

        public async Task<Uri> Put(Stream stream, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel) {
            var container = ContainerNameFromUri(uri);
            var containerRef = client.GetContainerReference(container);

            await containerRef.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, Options, Context);
            var blobName = BlobNameFromUri(uri);

            var blobRef = containerRef.GetBlockBlobReference(blobName);

            blobRef.Properties.ContentType = contentType.ToString();
            blobRef.Properties.ContentDisposition = contentDisposition.ToString();

            await blobRef.UploadFromStreamAsync(stream);
            return blobRef.Uri;
        }

        public async Task<bool> Remove(Uri uri, CancellationToken cancel) {
            var blobRef = await client.GetBlobReferenceFromServerAsync(uri);
            return await blobRef.DeleteIfExistsAsync();
        }

        public async Task<Uri> TemporaryLink(AccessLevel accessLevel, Uri uri, DateTime start, DateTime expiry, CancellationToken cancel) {
            if (!uri.IsAbsoluteUri) {
                uri = new Uri(BaseUri, uri);
            }

            var sasConstraints = new SharedAccessBlobPolicy {
                Permissions = (SharedAccessBlobPermissions)(int)accessLevel,
                //SharedAccessStartTime = start,
                SharedAccessExpiryTime = expiry
            };

            var containername = this.ContainerNameFromUri(uri);
            var container = client.GetContainerReference(containername);

            string token;
            // We are dealing with folder accesss
            if (uri.AbsoluteUri.EndsWith(containername)) {

                await container.CreateIfNotExistsAsync();
                token = container.GetSharedAccessSignature(sasConstraints);

            } else {

                var blobref = new CloudBlockBlob(uri, this.client);

                if (!await blobref.ExistsAsync()) {
                    throw new Exception("BLOB WAS NOT CREATED");
                }

                token = blobref.GetSharedAccessSignature(sasConstraints);
            }
            var builder = new UriBuilder(uri) {
                Query = token
            };

            return builder.Uri;
        }

        private string ContainerNameFromUri(Uri uri) {
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


        private string BlobNameFromUri(Uri uri) {
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
