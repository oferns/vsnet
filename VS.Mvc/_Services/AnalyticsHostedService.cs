
namespace VS.Mvc._Services {
    using MediatR;
    using SimpleInjector;
    using SimpleInjector.Lifestyles;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Abstractions.Logging;
    using VS.Core.Analytics;
    using VS.Core.Storage;

    public class AnalyticsHostedService : HostedService {
        private readonly IMediator mediator;
        private readonly IProducerConsumerCollection<UserEvent> collection;
        private readonly ILog log;
        private readonly Container container;

        public AnalyticsHostedService(IMediator mediator, IProducerConsumerCollection<UserEvent> collection, ILog log, Container container) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.container = container;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
            using (AsyncScopedLifestyle.BeginScope(this.container)) {
                while (!cancellationToken.IsCancellationRequested) {
                    await DoWork();
                    await Task.Delay(500);
                    await ExecuteAsync(cancellationToken);
                }
            }
        }

        private async Task DoWork() {
            log.LogInfo("Doing background work");
            
            while (this.collection.TryTake(out var @event)) {
                var uri = new Uri($"/vs-anal/{Guid.NewGuid()}", UriKind.Relative);
                var sb = new StringBuilder("[");
                sb.Append(string.Join(',', @event.Events));
                sb.Append("]");
                using var str = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
                await mediator.Send(new Put(uri, str, new ContentDisposition(), new ContentType("application/json")), CancellationToken.None);
            }

            log.LogInfo("Ending background work");

        }
    }
}
