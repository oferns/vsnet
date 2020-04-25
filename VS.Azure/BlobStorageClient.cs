namespace VS.Azure {
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Abstractions.Storage;
    using VS.Abstractions.Storage.Paging;

    public class BlobStorageClient : IStorageClient {
        
        private readonly CloudBlobClient client;
        private readonly ISerializer serializer;
        private OperationContext context;
        private BlobRequestOptions Options => new BlobRequestOptions { };

        public BlobStorageClient(CloudBlobClient client, ISerializer serializer) {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        private OperationContext Context => context ??= new OperationContext { LogLevel = (LogLevel)1 };

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
                    return new PagedIndex(Array.Empty<IndexItem>(), pageSize, null);
                }
            }

            var blobName = BlobNameFromUri(prefix);
            var resultsSegment = await containerRef.ListBlobsSegmentedAsync(blobName, true, BlobListingDetails.Metadata, pageSize, ctoken, Options, Context, cancel);


            var items = new List<IndexItem>();

            foreach (var result in resultsSegment.Results) {
                if (result is CloudBlockBlob blob) {
                    items.Add(new IndexItem(result.Uri, new ContentDisposition(blob.Properties.ContentDisposition), new ContentType(blob.Properties.ContentType)));
                } // TODO: Is it always one of these?                       
            }

            return new PagedIndex(items, pageSize, token, resultsSegment.ContinuationToken?.NextMarker);
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

        public async Task<Uri> TemporaryLink(AccessLevel accessLevel, Uri uri, DateTimeOffset start, DateTimeOffset expiry, CancellationToken cancel) {
            if (!uri.IsAbsoluteUri) {
                uri = new Uri(BaseUri, uri);
            }

            var sasConstraints = new SharedAccessBlobPolicy {
                Permissions = (SharedAccessBlobPermissions)(int)accessLevel,
                //SharedAccessStartTime = start,
                SharedAccessExpiryTime = expiry.LocalDateTime
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

            var trimmedParts = new string[parts.Length - 1];
            Array.Copy(parts, 1, trimmedParts, 0, trimmedParts.Length);
            return string.Join('/', trimmedParts);
        }

        public Task<Uri> CreateContainer(AccessLevel accessLevel, string containerName, CancellationToken cancel) {
            throw new NotImplementedException();
        }


        public async Task<T> GetObject<T>(Uri uri, CancellationToken cancel) where T : class {
            using var objStream = await Get(uri, cancel);
            return await this.serializer.Deserialize<T>(objStream, cancel).AsTask();
        }

        public async Task<Uri> PutObject<T>(T @object, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel) where T : class {
            using var stream = new MemoryStream();
            await serializer.SerializeToStream(@object, stream, cancel);
            return await Put(stream, uri, contentDisposition, contentType, cancel);
        }
    }
}
