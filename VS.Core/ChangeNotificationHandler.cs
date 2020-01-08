
namespace VS.Core {

    using MediatR;
    using MessagePack;
    using MessagePack.Resolvers;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;

    public class ChangeNotificationHandler<T> : INotificationHandler<ChangeNotification<T>> {
        private readonly ILogger log;
        private readonly IContext context;

        public ChangeNotificationHandler(ILogger<ChangeNotificationHandler<T>> log, IContext context) {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task Handle(ChangeNotification<T> notification, CancellationToken cancel) {
            log.LogInformation($"USER: {context.User.Identity.Name}");

            log.LogInformation($"REQUEST: {notification.Request.GetType().FullName}");
            log.LogInformation($"RESULT: {notification.Result.GetType().FullName}");

#if DEBUG                       
            var result = MessagePackSerializer.SerializeToJson<T>(notification.Result, null, cancel);
            log.LogInformation($"{result}");
#endif
            return Task.CompletedTask;
        }
    }
}
