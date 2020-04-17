namespace VS.Mvc._Extensions {

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using VS.Core.Identity;

    public class MultiTenantRazorViewEngine : RazorViewEngine {
        public MultiTenantRazorViewEngine(IHttpContextAccessor contextAccessor, IRazorPageFactoryProvider pageFactory, IRazorPageActivator pageActivator, HtmlEncoder htmlEncoder, IOptions<RazorViewEngineOptions> optionsAccessor, ILoggerFactory loggerFactory, DiagnosticListener diagnosticListener)
            : base(pageFactory, pageActivator, htmlEncoder, optionsAccessor, loggerFactory, diagnosticListener) {

            if (contextAccessor is null) {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            //	Dirty hack: setting RazorViewEngine.ViewLookupCache property that does not have a setter.
            var field = typeof(RazorViewEngine).GetField("<ViewLookupCache>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(this, new MultiTenantMemoryCache(contextAccessor));

            //	Asserting that ViewLookupCache property was set to instance of MultiTenantMemoryCache
            if (ViewLookupCache.GetType() != typeof(MultiTenantMemoryCache)) {
                throw new InvalidOperationException("Failed to set multi-tenant memory cache");
            }
        }



        


        private class MultiTenantMemoryCache : IMemoryCache {
            private readonly IHttpContextAccessor contextAccessor;

            private static IDictionary<string, IMemoryCache> hostCaches = new Dictionary<string, IMemoryCache> { { "default", new MemoryCache(new MemoryCacheOptions()) } };
            private static object hostCachesLock = new object();

            internal MultiTenantMemoryCache(IHttpContextAccessor contextAccessor) {
                this.contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            }

            public ICacheEntry CreateEntry(object key) {
                return GetCurrentTenantCache().CreateEntry(key);
            }


            public void Remove(object key) {
                GetCurrentTenantCache().Remove(key);
            }

            public bool TryGetValue(object key, out object value) {
                return GetCurrentTenantCache().TryGetValue(key, out value);
            }

            private IMemoryCache GetCurrentTenantCache() {

                if (contextAccessor.HttpContext.Items["HostIdentity"] is ClaimsIdentity hostId &&
                    hostId.HasClaim(c => c.Type.Equals(IdClaimTypes.HostIdentifier, StringComparison.InvariantCulture))) {

                    var host = hostId.FindFirst(IdClaimTypes.HostIdentifier).Value;

                    if (hostCaches.ContainsKey(host)) {
                        return hostCaches[host];
                    }

                    lock (hostCachesLock) {
                        if (hostCaches.ContainsKey(host)) {
                            return hostCaches[host];
                        }
                        return (hostCaches[host] = new MemoryCache(new MemoryCacheOptions()));
                    }
                }

                return hostCaches["default"];

            }

            public void Dispose() {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing) {
                if (disposing) {
                    foreach (var cache in hostCaches) {
                        cache.Value.Dispose();
                    }
                }
            }

        }
    }
}
