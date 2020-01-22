namespace VS.Core {

    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;

    public class ChangeNotificationHandler<T> : INotificationHandler<ChangeNotification<T>> {
        private readonly ILogger log;
        private readonly IContext context;
        private readonly ISerializer serializer;

        public ChangeNotificationHandler(ILogger<ChangeNotificationHandler<T>> log, IContext context, ISerializer serializer) {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task Handle(ChangeNotification<T> notification, CancellationToken cancel) {
            log.LogInformation($"USER: {context.User.Identity.Name}");

            log.LogInformation($"REQUEST: {notification.Request.GetType().FullName}");
            log.LogInformation($"RESULT: {notification.Result.GetType().FullName}");

#if DEBUG                       
            var result = await serializer.SerializeToJson<T>(notification.Result, cancel);
            log.LogInformation($"{result}");
#else
            return Task.CompletedTask;      
#endif
        }
    }
}
