namespace VS.Mvc.Components.Razor.Runtime {

    using Microsoft.AspNetCore.Razor.Language;
    using Microsoft.Extensions.FileProviders;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class FileProviderRazorProjectFileSystem : RazorProjectFileSystem {
        private const string RazorFileExtension = ".cshtml";
        private readonly IFileProvider _fileProvider;
        private readonly string rootPath;

        public FileProviderRazorProjectFileSystem(string rootPath) {
            this.rootPath = rootPath;            
            _fileProvider = new PhysicalFileProvider(rootPath);
        }

        public IFileProvider FileProvider => _fileProvider;

        public string Root => rootPath;

        [Obsolete("Use GetItem(string path, string fileKind) instead.")]
        public override RazorProjectItem GetItem(string path) {
            return GetItem(path, fileKind: null);
        }

        public override RazorProjectItem GetItem(string path, string fileKind) {
            path = NormalizeAndEnsureValidPath(path);
            var fileInfo = FileProvider.GetFileInfo(path);
            return new FileProviderRazorProjectItem(fileInfo, basePath: string.Empty, filePath: path, root: null, fileKind);
        }
         
        public override IEnumerable<RazorProjectItem> EnumerateItems(string path) {
            path = NormalizeAndEnsureValidPath(path);
            return EnumerateFiles(FileProvider.GetDirectoryContents(path), path, prefix: string.Empty);
        }

        private IEnumerable<RazorProjectItem> EnumerateFiles(IDirectoryContents directory, string basePath, string prefix) {
            if (directory.Exists) {
                foreach (var fileInfo in directory) {
                    if (fileInfo.IsDirectory) {
                        var relativePath = prefix + "/" + fileInfo.Name;
                        var subDirectory = FileProvider.GetDirectoryContents(JoinPath(basePath, relativePath));
                        var children = EnumerateFiles(subDirectory, basePath, relativePath);
                        foreach (var child in children) {
                            yield return child;
                        }
                    } else if (string.Equals(RazorFileExtension, Path.GetExtension(fileInfo.Name), StringComparison.OrdinalIgnoreCase)) {
                        var filePath = prefix + "/" + fileInfo.Name;

                        yield return new FileProviderRazorProjectItem(fileInfo, basePath, filePath: filePath, null);
                    }
                }
            }
        }

        private static string JoinPath(string path1, string path2) {
            var hasTrailingSlash = path1.EndsWith("/", StringComparison.Ordinal);
            var hasLeadingSlash = path2.StartsWith("/", StringComparison.Ordinal);
            if (hasLeadingSlash && hasTrailingSlash) {
                return path1 + path2.Substring(1);
            } else if (hasLeadingSlash || hasTrailingSlash) {
                return path1 + path2;
            }

            return path1 + "/" + path2;
        }
    }
}
