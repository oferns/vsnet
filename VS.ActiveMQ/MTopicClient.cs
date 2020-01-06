namespace VS.ActiveMQ {

    using Apache.NMS;
    using System;
    using System.Text.Json;
    using VS.Abstractions.Messaging;

    public class MTopicClient : ITopicClient<ITopic> {

        private readonly ISession session;

        public MTopicClient(ISession session) {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Message<M> Broadcast<M>(string topic, Message<M> message) where M : class {
            var dest = this.session.GetTopic(topic);
            using var producer = session.CreateProducer(dest);
            var mess = this.session.CreateTextMessage(JsonSerializer.Serialize(message.Body));
            mess.NMSCorrelationID = message.CorrelationId;
            producer.Send(mess);
            return message;
        }
    }
}