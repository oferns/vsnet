namespace VS.Mvc._Startup {

    using Microsoft.Extensions.Configuration;
    using Serilog;
    using SimpleInjector;
    using StackExchange.Redis;
    using VS.Abstractions.Caching;
    using VS.Core.Caching;
    using VS.Core.Local.Cache;
    using VS.Redis;

    public static class AppCaching {


        public static Container AddCaching(this Container container, IConfiguration config, ILogger log) {
            if (container is null) {
                throw new System.ArgumentNullException(nameof(container));
            }

            if (config is null) {
                throw new System.ArgumentNullException(nameof(config));
            }

            if (log is null) {
                throw new System.ArgumentNullException(nameof(log));
            }

            var redisconfig = config.GetSection("RedisOptions").Get<RedisOptions>();


            if (redisconfig is null) {
                log.Warning("No Redis configuration found. Skipping Redis services.");
            }



            container.Register(typeof(ICacheClient<>), typeof(CombinedCacheClient<>));
            container.Collection.Append(typeof(ICacheClient<>), typeof(MemoryCacheClient<>));

            if (redisconfig is object) {

                foreach (var host in redisconfig.Hosts) {
                    redisconfig.ConfigurationOptions.EndPoints.Add(host);
                }

                container.RegisterSingleton<IConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisconfig.ConfigurationOptions));
                container.Register<IDatabase>(() => container.GetInstance<IConnectionMultiplexer>().GetDatabase());

                container.Collection.Append(typeof(ICacheClient<>), typeof(RedisCacheClient<>));
            }

            return container;
        }
    }
}
 