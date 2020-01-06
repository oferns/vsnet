using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VS.Mvc._Services {
    public class CacheBusterSubcription : IHostedService {
        private readonly IServiceProvider services;

        public CacheBusterSubcription(IServiceProvider services) {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }


        public Task StartAsync(CancellationToken cancel) {
            throw new NotImplementedException();
        }

        
        
        public Task StopAsync(CancellationToken cancel) {
            throw new NotImplementedException();
        }
    }
}
