namespace VS.Core.Storage {
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.FileProviders;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
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
            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            var info = this.provider.GetFileInfo(uri.AbsoluteUri);
            return Task.FromResult(info.Exists);

        }

        public Task<Stream> Get(Uri uri, CancellationToken cancel) {
            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            var info = this.provider.GetFileInfo(uri.AbsoluteUri);
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
            if (!uri.IsAbsoluteUri) {
                uri = new Uri(this.BaseUri, uri);
            }

            var info = this.provider.GetFileInfo(uri.AbsoluteUri);

            if (!info.Exists) {
                return Task.FromResult(new PagedIndex(Enumerable.Empty<IndexItem>(), pageSize));
            }

            if (!info.IsDirectory) {
                //           return Task.FromResult(new PagedIndex(new[] { GetItemFromUri(uri) }, pageSize));
            }

            var ite = this.provider.GetDirectoryContents(uri.AbsoluteUri);


            var dirInfo = new DirectoryInfo(uri.AbsoluteUri);

            return Task.FromResult(default(PagedIndex));

        }

        private IEnumerable<IndexItem> FlattenDirectory(IFileInfo info) {
            if (info == null) {
                yield break;
            }

            if (!info.IsDirectory) {
                yield return GetItemFromInfo(info);
            }

            foreach (var item in this.provider.GetDirectoryContents(info.PhysicalPath)) {
                if (item.IsDirectory) {
                    foreach (var subitem in FlattenDirectory(item)) {

                    }
                }
            }


        }

        public async Task<Uri> Put(Stream stream, Uri uri, ContentDisposition contentDisposition, ContentType contentType, CancellationToken cancel) {
            bool isRelative = !uri.IsAbsoluteUri;
            if (isRelative) {
                uri = new Uri(this.BaseUri, uri);
            }

            if (!await Exists(uri, cancel)) {
                new FileInfo(uri.LocalPath).Directory.Create();
            }

            using FileStream writer = new FileStream(uri.LocalPath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(writer);

            return uri;

        }

        public Task<bool> Remove(Uri uri, CancellationToken cancel) {
            throw new NotImplementedException();
        }

        public Task<Uri> TemporaryLink(AccessLevel accessLevel, Uri path, DateTime start, DateTime expiry, CancellationToken cancel) {
            if (!path.IsAbsoluteUri) {
                path = new Uri(this.BaseUri, path);
            }
            return Task.FromResult(path);
        }

        /// <summary>                                                                                                                                                    ks*
        /// 
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


        //private void GetFiles(IFileInfo fileInfo) {

        //    var contents = this.provider.GetDirectoryContents("");

        //    foreach (var content in contents) {
        //        if (!content.IsDirectory && content.Name.ToLower().EndsWith(".pdf")) {
        //            files.Add(content);
        //        } else {
        //            GetFiles(content.PhysicalPath, files);
        //        }
        //    }
        //}
    }
}
