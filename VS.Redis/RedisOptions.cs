namespace VS.Redis {
    using System.Collections;
    using System.Collections.Generic;
    using StackExchange.Redis;

    public class RedisOptions {

        public IEnumerable<string> Hosts { get; set; }

        public ConfigurationOptions ConfigurationOptions { get; set; }

    }
}
