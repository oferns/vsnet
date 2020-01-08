namespace VS.Core.Messaging.Queue {

    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions.Messaging;

    public class Enqueue<T> : IRequest<Message<T>> where T : class {

        public Enqueue(T Message) {
            this.Message = Message ?? throw new ArgumentNullException(nameof(Message));
        }

        public T Message { get; }
    }
}
