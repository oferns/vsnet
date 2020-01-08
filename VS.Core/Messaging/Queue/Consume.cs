namespace VS.Core.Messaging.Queue{
 
    using MediatR;
    using System;
    using VS.Abstractions.Messaging;

    public class Consume<T> : IRequest<T> where T : class {

        public Consume(Message<T> Message) {
            this.Message = Message ?? throw new ArgumentNullException(nameof(Message));
        }

        public Message<T> Message { get; }
    }
}