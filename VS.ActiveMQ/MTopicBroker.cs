namespace VS.ActiveMQ {

    using Apache.NMS;
    using Apache.NMS.ActiveMQ.Commands;
    using System;
    using System.Collections.Generic;
    using VS.Abstractions.Messaging;

    public class MTopicBroker : ITopicBroker<ITopic> {

        private const string TOPIC_ADV = "ActiveMQ.Advisory.Topic";
        private readonly ISession session;

        private static List<ITopic> internalTopicList;
        private static readonly object listLock = new object();

        public MTopicBroker(ISession session) {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public ITopic Create(string topicName) {
            return this.session.GetTopic(topicName);
        }

        public ITopic Destroy(string topicName) {
            this.session.DeleteTopic(topicName);
            return new ActiveMQTopic(topicName);
        }

        public IEnumerable<ITopic> List(bool forceRefresh) {
            if (forceRefresh || internalTopicList is null) {
                using IDestination dest = session.GetTopic(TOPIC_ADV);
                using IMessageConsumer consumer = session.CreateConsumer(dest);
                IMessage advisory;

                var tmpList = new List<ITopic>();
                while ((advisory = consumer.Receive(TimeSpan.FromMilliseconds(2000))) != null) {
                    ActiveMQMessage amqMsg = advisory as ActiveMQMessage;

                    if (amqMsg.DataStructure != null) {
                        var info = amqMsg.DataStructure as DestinationInfo;
                        if (info != null) {
                            tmpList.Add(new ActiveMQTopic(info.Destination.PhysicalName));
                        }
                    }
                }

                lock (listLock) {
                    internalTopicList ??= new List<ITopic>();
                    internalTopicList.Clear();
                    internalTopicList.AddRange(tmpList);
                }
            }
            return internalTopicList;
        }
    }
}