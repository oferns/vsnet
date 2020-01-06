namespace VS.Core.Cache {
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions.Cache;

    public class MemoryCacheClient : ICacheClient {
        private readonly IMemoryCache cache;

        public MemoryCacheClient(IMemoryCache cache) {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
    }
}
