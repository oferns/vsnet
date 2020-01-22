
namespace VS.Core.Messaging.Queue {
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Messaging;

    public class ImmediateQueueClient : IQueueClient {
        
        private readonly IMediator mediator;

        public ImmediateQueueClient(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<Message<M>> Send<M>(string queue, Message<M> message, CancellationToken cancel) where M : class {
            _ = await mediator.Send(new Consume<M>(message), cancel);
            return message;
        }
    }
}
