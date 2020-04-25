﻿
namespace VS.Mvc.Components.Razor.Runtime {

    using Microsoft.AspNetCore.Razor.Language;
    using Microsoft.Extensions.FileProviders;
    using System;
    using System.IO;

    public class FileProviderRazorProjectItem : RazorProjectItem {
        private string _root;
        private string _relativePhysicalPath;
        private bool _isRelativePhysicalPathSet;

        public FileProviderRazorProjectItem(IFileInfo fileInfo, string basePath, string filePath, string root) : this(fileInfo, basePath, filePath, root, fileKind: null) {
        }

        public FileProviderRazorProjectItem(IFileInfo fileInfo, string basePath, string filePath, string root, string fileKind) {
            FileInfo = fileInfo;
            BasePath = basePath;
            FilePath = filePath;
            FileKind = fileKind ?? FileKinds.GetFileKindFromFilePath(filePath);
            _root = root;
        }

        public IFileInfo FileInfo { get; }

        public override string BasePath { get; }

        public override string FilePath { get; }

        public override string FileKind { get; }

        public override bool Exists => FileInfo.Exists;

        public override string PhysicalPath => FileInfo.PhysicalPath;

        public override string RelativePhysicalPath {
            get {
                if (!_isRelativePhysicalPathSet) {
                    _isRelativePhysicalPathSet = true;

                    if (Exists) {
                        if (_root != null &&
                            !string.IsNullOrEmpty(PhysicalPath) &&
                            PhysicalPath.StartsWith(_root, StringComparison.OrdinalIgnoreCase) &&
                            PhysicalPath.Length > _root.Length &&
                            (PhysicalPath[_root.Length] == Path.DirectorySeparatorChar || PhysicalPath[_root.Length] == Path.AltDirectorySeparatorChar)) {
                            _relativePhysicalPath = PhysicalPath.Substring(_root.Length + 1); // Include leading separator
                        }
                    }
                }

                return _relativePhysicalPath;
            }
        }

        public override Stream Read() {
            return FileInfo.CreateReadStream();
        }
    }
}