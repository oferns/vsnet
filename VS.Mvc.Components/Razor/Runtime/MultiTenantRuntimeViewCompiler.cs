namespace VS.Mvc.Components.Razor.Runtime {
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Razor.Compilation;
    using Microsoft.AspNetCore.Razor.Hosting;
    using Microsoft.AspNetCore.Razor.Language;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Emit;
    using Microsoft.CodeAnalysis.Text;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class MultiTenantRuntimeViewCompiler : IViewCompiler {

        private readonly object cacheLock = new object();
        private readonly IDictionary<string, CompiledViewDescriptor> compiledViews;

        private readonly IMemoryCache cache;

        private readonly ConcurrentDictionary<string, string> normalizedPathCache;
        private readonly IDictionary<string, RazorProjectEngine> projectEngines;

        private readonly ILogger logger;
        private readonly CSharpCompiler csharpCompiler;

        public MultiTenantRuntimeViewCompiler(IDictionary<string, RazorProjectEngine> projectEngines,
            CSharpCompiler csharpCompiler,
            IList<CompiledViewDescriptor> compiledViews,
            ILogger logger) {

            this.projectEngines = projectEngines;
            this.csharpCompiler = csharpCompiler;
            this.logger = logger;

            // This is our L0 cache, and is a durable store. Views migrate into the cache as they are requested
            // from either the set of known precompiled views, or by being compiled.
            cache = new MemoryCache(new MemoryCacheOptions());


            normalizedPathCache = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

            // We need to validate that the all of the precompiled views are unique by path (case-insensitive).
            // We do this because there's no good way to canonicalize paths on windows, and it will create
            // problems when deploying to linux. Rather than deal with these issues, we just don't support
            // views that differ only by case.
            this.compiledViews = new Dictionary<string, CompiledViewDescriptor>(
                compiledViews.Count,
                StringComparer.OrdinalIgnoreCase);

            foreach (var compiledView in compiledViews) {

                if (!this.compiledViews.ContainsKey(compiledView.RelativePath)) {
                    // View ordering has precedence semantics, a view with a higher precedence was not
                    // already added to the list.


                    this.compiledViews.Add(compiledView.RelativePath, compiledView);
                }
            }
        }

        public Task<CompiledViewDescriptor> CompileAsync(string relativePath) {
            if (relativePath == null) {
                throw new ArgumentNullException(nameof(relativePath));
            }

            // Attempt to lookup the cache entry using the passed in path. This will succeed if the path is already
            // normalized and a cache entry exists.
            if (cache.TryGetValue(relativePath, out Task<CompiledViewDescriptor> cachedResult)) {
                return cachedResult;
            }

            var normalizedPath = GetNormalizedPath(relativePath);
            if (cache.TryGetValue(normalizedPath, out cachedResult)) {
                return cachedResult;
            }

            // Entry does not exist. Attempt to create one.
            cachedResult = OnCacheMiss(normalizedPath);
            return cachedResult;
        }

        private IFileProvider GetProviderFromRazorProjectEngine(RazorProjectEngine engine) {
            return ((FileProviderRazorProjectFileSystem)engine.FileSystem).FileProvider;

        }

        private Task<CompiledViewDescriptor> OnCacheMiss(string normalizedPath) {
            ViewCompilerWorkItem item;
            TaskCompletionSource<CompiledViewDescriptor> taskSource;
            MemoryCacheEntryOptions cacheEntryOptions;


            // Safe races cannot be allowed when compiling Razor pages. To ensure only one compilation request succeeds
            // per file, we'll lock the creation of a cache entry. Creating the cache entry should be very quick. The
            // actual work for compiling files happens outside the critical section.
            lock (cacheLock) {

                // Double-checked locking to handle a possible race.
                if (cache.TryGetValue(normalizedPath, out Task<CompiledViewDescriptor> result)) {
                    return result;
                }

                if (compiledViews.TryGetValue(normalizedPath, out var precompiledView)) {
                    // _logger.ViewCompilerLocatedCompiledViewForPath(normalizedPath);
                    item = CreatePrecompiledWorkItem(normalizedPath, precompiledView);
                } else {
                    item = CreateRuntimeCompilationWorkItem(normalizedPath);
                }

                var tokens = new List<IChangeToken>();
                taskSource = new TaskCompletionSource<CompiledViewDescriptor>(creationOptions: TaskCreationOptions.RunContinuationsAsynchronously);

                tokens.AddRange(item.ExpirationTokens);
            
                if (!item.SupportsCompilation) {
                    taskSource.SetResult(item.Descriptor);
                }
                
                cacheEntryOptions = new MemoryCacheEntryOptions();
                
                foreach (var token in tokens) {
                    cacheEntryOptions.ExpirationTokens.Add(token);
                }
                cache.Set(normalizedPath, taskSource.Task, cacheEntryOptions);
            }


            // Now the lock has been released so we can do more expensive processing.
            if (item.SupportsCompilation) {
                Debug.Assert(taskSource != null);

                if (item.Descriptor?.Item != null) {
                    var engine = projectEngines[item.Descriptor?.Item.Type.Assembly.GetName().Name];

                    // If the item has checksums to validate, we should also have a precompiled view.

                    if (ChecksumValidator.IsItemValid(engine.FileSystem, item.Descriptor.Item)) {
                        Debug.Assert(item.Descriptor != null);

                        taskSource.SetResult(item.Descriptor);
                        return taskSource.Task;
                    }
                }

                // _logger.ViewCompilerInvalidingCompiledFile(item.NormalizedPath);

                var exceptions = new List<Exception>();

                foreach (var engine in projectEngines) {
                    try {
                        var descriptor = CompileAndEmit(normalizedPath, engine.Value);
                        descriptor.ExpirationTokens = cacheEntryOptions.ExpirationTokens;
                        taskSource.SetResult(descriptor);
                        return taskSource.Task;
                    } catch (Exception ex) {
                        exceptions.Add(ex);
                        logger.LogError(ex, "Razor blowup");
                    }
                }
                if (exceptions.Count > 0) {
                    taskSource.SetException(exceptions.AsEnumerable());                                                
                }
            }

            return taskSource.Task;
        }

        private ViewCompilerWorkItem CreatePrecompiledWorkItem(string normalizedPath, CompiledViewDescriptor precompiledView) {
            // We have a precompiled view - but we're not sure that we can use it yet.
            //
            // We need to determine first if we have enough information to 'recompile' this view. If that's the case
            // we'll create change tokens for all of the files.
            //
            // Then we'll attempt to validate if any of those files have different content than the original sources
            // based on checksums.
            if (precompiledView.Item == null || !ChecksumValidator.IsRecompilationSupported(precompiledView.Item)) {
                return new ViewCompilerWorkItem {
                    // If we don't have a checksum for the primary source file we can't recompile.
                    SupportsCompilation = false,
                    ExpirationTokens = Array.Empty<IChangeToken>(), // Never expire because we can't recompile.
                    Descriptor = precompiledView, // This will be used as-is.
                };
            }

            var item = new ViewCompilerWorkItem {
                SupportsCompilation = true,
                Descriptor = precompiledView, // This might be used, if the checksums match.
                // Used to validate and recompile
                NormalizedPath = normalizedPath,
                ExpirationTokens = GetExpirationTokens(precompiledView)
            };

            // We also need to create a new descriptor, because the original one doesn't have expiration tokens on
            // it. These will be used by the view location cache, which is like an L1 cache for views (this class is
            // the L2 cache).
            item.Descriptor = new CompiledViewDescriptor() {
                ExpirationTokens = item.ExpirationTokens,
                Item = precompiledView.Item,
                RelativePath = precompiledView.RelativePath,
            };

            return item;
        }

        private ViewCompilerWorkItem CreateRuntimeCompilationWorkItem(string normalizedPath) {
            var allTokens = new List<IChangeToken>();
            var exists = false;
            foreach (var engine in this.projectEngines) {
                var fileProvider = GetProviderFromRazorProjectEngine(engine.Value);
                IList<IChangeToken> expirationTokens = new List<IChangeToken> {
                    fileProvider.Watch(normalizedPath),
                };

                var projectItem = engine.Value.FileSystem.GetItem(normalizedPath, fileKind: null);
                if (projectItem.Exists) {
                    exists = true;
                    // _logger.ViewCompilerCouldNotFindFileAtPath(normalizedPath);
                    GetChangeTokensFromImports(expirationTokens, projectItem);
                }

                allTokens.AddRange(expirationTokens);
                // _logger.ViewCompilerFoundFileToCompile(normalizedPath);
            }

            return new ViewCompilerWorkItem() {
                SupportsCompilation = exists,
                NormalizedPath = normalizedPath,
                ExpirationTokens = allTokens,
                Descriptor = exists ? default : new CompiledViewDescriptor() {
                    RelativePath = normalizedPath,
                    ExpirationTokens = allTokens,
                } 
            };
        }

        private IList<IChangeToken> GetExpirationTokens(CompiledViewDescriptor precompiledView) {
            var checksums = precompiledView.Item.GetChecksumMetadata();
            var expirationTokens = new List<IChangeToken>(checksums.Count);
            
            foreach (var engine in projectEngines) {
                            
                var provider = GetProviderFromRazorProjectEngine(engine.Value);
                for (var i = 0; i < checksums.Count; i++) {
                    // We rely on Razor to provide the right set of checksums. Trust the compiler, it has to do a good job,
                    // so it probably will.

                    expirationTokens.Add(provider.Watch(checksums[i].Identifier));
                }     
            }
            return expirationTokens;
        }

        private void GetChangeTokensFromImports(IList<IChangeToken> expirationTokens, RazorProjectItem projectItem) {
            // OK this means we can do compilation. For now let's just identify the other files we need to watch
            // so we can create the cache entry. Compilation will happen after we release the lock.

            foreach (var engine in this.projectEngines) {
                var importFeature = engine.Value.ProjectFeatures.OfType<IImportProjectFeature>().ToArray();
                foreach (var feature in importFeature) {
                    foreach (var file in feature.GetImports(projectItem)) {
                        if (file.FilePath != null) {
                            expirationTokens.Add(GetProviderFromRazorProjectEngine(engine.Value).Watch(file.FilePath));
                        }
                    }
                }
            }
        }

        protected virtual CompiledViewDescriptor CompileAndEmit(string relativePath, RazorProjectEngine engine) {
            var projectItem = engine.FileSystem.GetItem(relativePath, fileKind: null);
            var codeDocument = engine.Process(projectItem);
            var cSharpDocument = codeDocument.GetCSharpDocument();

            if (cSharpDocument.Diagnostics.Count > 0) {
                //throw CompilationFailedExceptionFactory.Create(
                //    codeDocument,
                //    cSharpDocument.Diagnostics);
                throw new ApplicationException("Compilation Failed");
            }

            var assembly = CompileAndEmit(codeDocument, cSharpDocument.GeneratedCode, engine);

            // Anything we compile from source will use Razor 2.1 and so should have the new metadata.
            var loader = new RazorCompiledItemLoader();
            var item = loader.LoadItems(assembly).SingleOrDefault();
            return new CompiledViewDescriptor(item);
        }

        internal Assembly CompileAndEmit(RazorCodeDocument codeDocument, string generatedCode, RazorProjectEngine engine) {
            // _logger.GeneratedCodeToAssemblyCompilationStart(codeDocument.Source.FilePath);

            var startTimestamp = logger.IsEnabled(LogLevel.Debug) ? Stopwatch.GetTimestamp() : 0;

            var assemblyName = Path.GetRandomFileName();
            var compilation = CreateCompilation(generatedCode, assemblyName);

            var emitOptions = csharpCompiler.EmitOptions;
            var emitPdbFile = csharpCompiler.EmitPdb && emitOptions.DebugInformationFormat != DebugInformationFormat.Embedded;

            using (var assemblyStream = new MemoryStream())
            using (var pdbStream = emitPdbFile ? new MemoryStream() : null) {
                var result = compilation.Emit(
                    assemblyStream,
                    pdbStream,
                    options: emitOptions);

                if (!result.Success) {
                    throw new ApplicationException("Compilation Failed");
                }

                assemblyStream.Seek(0, SeekOrigin.Begin);
                pdbStream?.Seek(0, SeekOrigin.Begin);

                var assembly = Assembly.Load(assemblyStream.ToArray(), pdbStream?.ToArray());
                //_logger.GeneratedCodeToAssemblyCompilationEnd(codeDocument.Source.FilePath, startTimestamp);

                return assembly;
            }
        }

        private CSharpCompilation CreateCompilation(string compilationContent, string assemblyName) {
            var sourceText = SourceText.From(compilationContent, Encoding.UTF8);
            var syntaxTree = csharpCompiler.CreateSyntaxTree(sourceText).WithFilePath(assemblyName);
            return csharpCompiler
                .CreateCompilation(assemblyName)
                .AddSyntaxTrees(syntaxTree);
        }

        private string GetNormalizedPath(string relativePath) {
            Debug.Assert(relativePath != null);
            if (relativePath.Length == 0) {
                return relativePath;
            }

            if (!normalizedPathCache.TryGetValue(relativePath, out var normalizedPath)) {
                normalizedPath = NormalizePath(relativePath);
                normalizedPathCache[relativePath] = normalizedPath;
            }

            return normalizedPath;
        }


        public static string NormalizePath(string path) {
            var addLeadingSlash = path[0] != '\\' && path[0] != '/';
            var transformSlashes = path.IndexOf('\\') != -1;

            if (!addLeadingSlash && !transformSlashes) {
                return path;
            }

            var length = path.Length;
            if (addLeadingSlash) {
                length++;
            }

            return string.Create(length, (path, addLeadingSlash), (span, tuple) => {
                var (pathValue, addLeadingSlashValue) = tuple;
                var spanIndex = 0;

                if (addLeadingSlashValue) {
                    span[spanIndex++] = '/';
                }

                foreach (var ch in pathValue) {
                    span[spanIndex++] = ch == '\\' ? '/' : ch;
                }
            });
        }

        private class ViewCompilerWorkItem {
            public bool SupportsCompilation { get; set; }

            public string NormalizedPath { get; set; }

            public IList<IChangeToken> ExpirationTokens { get; set; }

            public CompiledViewDescriptor Descriptor { get; set; }
        }
    }
}