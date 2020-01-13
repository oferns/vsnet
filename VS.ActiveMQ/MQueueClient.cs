namespace VS.ActiveMQ {
    using Apache.NMS;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using VS.Abstractions.Messaging;

    public class MQueueClient : IQueueClient<IQueue> {

        private readonly ISession session;

        public MQueueClient(ISession session) {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Message<M> Send<M>(string queue, Message<M> message) where M : class {

            var dest = this.session.GetQueue(queue);
            using var producer = session.CreateProducer(dest);
            var mess = this.session.CreateTextMessage(JsonSerializer.Serialize(message.Body));
            mess.NMSCorrelationID = message.CorrelationId;
            producer.Send(mess, MsgDeliveryMode.Persistent,MsgPriority.Normal, TimeSpan.FromSeconds(10));
            return message;
        }
    }
}
