
namespace VS.Core.Messaging.Queue {

    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Messaging;

    public class EnqueueHandler<T> : IRequestHandler<Enqueue<T>, Message<T>> where T : class {

        private readonly IMediator mediator;

        public EnqueueHandler(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Message<T>> Handle(Enqueue<T> request, CancellationToken cancel) {
            var message = new Message<T>(request.Message, "test");
            await mediator.Send(new Consume<T>(message), cancel);
            return message;
        }
    }
}
