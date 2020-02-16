namespace VS.Core.Storage {
    using System;
    using System.Collections.Generic;
    using System.IO;    
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.FileProviders;
    using VS.Abstractions.Storage;
    using VS.Abstractions.Storage.Paging;

    /// <summary>
    /// Base storage client that uses the machine temp directory if no config is provided
    /// </summary>
    public class FileStorageClient : IStorageClient {

        private readonly IFileProvider provider;
        private readonly IContentTypeProvider typeProvider;

        public FileStorageClient(IFileProvider provider, IContentTypeProvider typeProvider) {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.typeProvider = typeProvider ?? throw new ArgumentNullException(nameof(typeProvider));
        }

        private Uri baseUri;
        public Uri BaseUri {
            get {
                if (baseUri is null) {
                    if (provider is PhysicalFileProvider pfile) {
                        return (baseUri = new Uri(pfile.Root));
                    }
                    return (baseUri = new Uri(Path.GetTempPath()));
                }
                return baseUri;
            }
        }

        public Task<bool> Exists(Uri uri, CancellationToken cancel) {
            if (uri.IsAbsoluteUri) {
                uri = uri.MakeRelativeUri(this.BaseUri);
            }

            var info = this.provider.GetFileInfo(uri.ToString());
            return Task.FromResult(info.Exists);

        }

        public Task<Stream> Get(Uri uri, CancellationToken cancel) {
            if (uri.IsAbsoluteUri) {
                uri = uri.MakeRelativeUri(this.BaseUri);
            }

            var info = this.provider.GetFileInfo(uri.ToString());
            return Task.FromResult(info.CreateReadStream());
        }

        /// <summary>
        /// This implementation will flatten directories to only list full file path names. If you pass a file then it will return 
        /// one result with a reference to that file.
        /// </summary>
        /// <param name="uri">the path</param>
        /// <param name="pageSize">the max number of items to return</param>
        /// <param name="token">the filename to start at</param>
        /// <param name="cancel">a cancellation token</param>
        /// <returns></returns>
        public Task<PagedIndex> Index(Uri uri, int pageSize, string token, CancellationToken cancel) {
            if (uri.IsAbsoluteUri) {
                uri = uri.MakeRelativeUri(this.BaseUri);
            }

            var info = this.provider.GetFileInfo(uri.ToString());

            if (info.Exists) {
                return Task.FromResult(new PagedIndex(new[] { GetItemFromInfo(info) }, pageSize));
            }

            var files = this.provider.GetDirectoryContents(uri.ToString());

            var items = new List<IndexItem>();

            var index = 1;

            foreach (var file in files) {
                if (index.Equals(pageSize)) break;
                items.Add(GetItemFromInfo(file));
                index++;
            }

            return Task.FromResult(new PagedIndex(items, pageSize, token));

        }
       
        public async Task<Uri> Put(Stream stream, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel) {
            if (uri.IsAbsoluteUri) {
                uri = uri.MakeRelativeUri(this.BaseUri);
            }

            if (!await Exists(uri, cancel)) {
                new FileInfo(uri.ToString()).Directory.Create();
            }

            using FileStream writer = new FileStream(new Uri(this.BaseUri, uri).LocalPath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(writer);

            return uri;
        }

        public async Task<bool> Remove(Uri uri, CancellationToken cancel) {
            return await Task.Factory.StartNew(() => {
                if (uri.IsAbsoluteUri) {
                    uri = uri.MakeRelativeUri(this.BaseUri);
                }

                var info = this.provider.GetFileInfo(uri.ToString());

                if (!info.Exists) {
                    return false;
                }

                File.Delete(info.PhysicalPath);
                return true;
            });
        }

        public Task<Uri> TemporaryLink(AccessLevel accessLevel, Uri uri, DateTime start, DateTime expiry, CancellationToken cancel) {
            if (uri.IsAbsoluteUri) {
                uri = uri.MakeRelativeUri(this.BaseUri);
            }
            return Task.FromResult(new Uri(this.BaseUri, uri));
        }

        /// <summary>                                                                                                                                                    
        /// Gets info about a local file and converts it to a <see cref="IndexItem"/>
        /// </summary>
        /// <param name="uri">The file Uri</param>
        /// <returns>A <see cref="IndexItem"/> representing the file, or null if it doesnt exist or is a directory</returns>
        private IndexItem GetItemFromInfo(IFileInfo info) {

            if (!info.Exists || info.IsDirectory) {
                return default(IndexItem);
            }

            var contenttype = default(ContentType);

            var contentdisposition = new ContentDisposition {
                CreationDate = File.GetCreationTime(info.PhysicalPath),
                FileName = info.Name,
                ModificationDate = info.LastModified.UtcDateTime,
                Size = info.Length
            };

            if (this.typeProvider.TryGetContentType(info.PhysicalPath, out var contenttypestring)) {
                contenttype = new ContentType(contenttypestring);
            }

            return new IndexItem(new Uri(info.PhysicalPath), contentdisposition, contenttype);
        }

        private IEnumerable<IFileInfo> GetFiles(IEnumerable<IFileInfo> dirContents) {
            foreach (var fileOrDir in dirContents) {
                if (fileOrDir.IsDirectory) {
                    GetFiles(this.provider.GetDirectoryContents(fileOrDir.PhysicalPath));
                }
                yield return fileOrDir;
            }
        }
    }
}
