namespace VS.Abstractions.Messaging {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public  class Message<T> where T: class {

        public Message(T body, string correlationId) {
            if (body is null) {
                throw new ArgumentNullException(nameof(body));
            }

            Body = body;
            CorrelationId = correlationId;
        }

        public string CorrelationId { get; }

        public T Body { get; }
    }
}
