namespace VS.Mvc._Startup {

    using Microsoft.Extensions.Configuration;
    using Serilog;
    using SimpleInjector;
    using StackExchange.Redis;
    using VS.Abstractions.Caching;
    using VS.Core.Caching;
    using VS.Core.Local.Cache;
    using VS.Redis;

    public static class Caching {


        public static Container AddCaching(this Container container, IConfiguration config, ILogger log) {

            var redisconfig = config.GetSection("RedisOptions").Get<RedisOptions>();


            if (redisconfig is null) {
                log.Warning("No Redis configuration found. Skipping Redis services.");
            }



            container.Register(typeof(ICacheClient<>), typeof(CombinedCacheClient<>));
            //container.Collection.Append(typeof(ICacheClient<>), typeof(MemoryCacheClient<>));

            if (redisconfig is object) {

                foreach (var host in redisconfig.Hosts) {
                    redisconfig.ConfigurationOptions.EndPoints.Add(host);
                }

                container.RegisterSingleton<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisconfig.ConfigurationOptions));
                container.Register<IDatabase>(() => container.GetInstance<ConnectionMultiplexer>().GetDatabase());

                container.Collection.Append(typeof(ICacheClient<>), typeof(RedisCacheClient<>));
            }

            return container;
        }
    }
}
