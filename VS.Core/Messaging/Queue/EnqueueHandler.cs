
namespace VS.Core.Messaging.Queue {

    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Abstractions.Messaging;

    public class EnqueueHandler<T> : IRequestHandler<Enqueue<T>, Message<T>> where T : class {

        private readonly IMediator mediator;
        private readonly IContext context;

        public EnqueueHandler(IMediator mediator, IContext context) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Message<T>> Handle(Enqueue<T> request, CancellationToken cancel) {
            var message = new Message<T>(request.Message, context.RequestId);
            _ = await mediator.Send(new Consume<T>(message), cancel);
            return message;
        }
    }
}
