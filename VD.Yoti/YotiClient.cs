namespace VD.Yoti {

    using System;
    using System.Net.Http;
    using Microsoft.Extensions.Logging;

    public class YotiClient {
        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger<YotiClient> log;

        public YotiClient(IHttpClientFactory clientFactory, ILogger<YotiClient> log) {
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }


    }
}
