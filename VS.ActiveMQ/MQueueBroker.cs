namespace VS.ActiveMQ {
    using Apache.NMS;
    using Apache.NMS.ActiveMQ.Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Abstractions.Messaging;

    public class MQueueBroker : IQueueBroker<IQueue> {

        private const string QUEUE_ADV = "ActiveMQ.Advisory.Queue";
        private readonly ISession session;

        private static List<IQueue> internalQueueList;
        private static readonly object listLock = new object();

        public MQueueBroker(ISession session) {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public IQueue Create(string queueName) {
            return this.session.GetQueue(queueName);
        }

        public IQueue Destroy(string queueName) {
            this.session.DeleteQueue(queueName);
            return new ActiveMQQueue(queueName);
        }

        public IEnumerable<IQueue> List(bool forceRefresh) {
            if (forceRefresh || internalQueueList is null) {
                using IDestination dest = session.GetTopic(QUEUE_ADV);
                using IMessageConsumer consumer = session.CreateConsumer(dest);
                IMessage advisory;
                
var tmpList = new List<IQueue>();
                while ((advisory = consumer.Receive(TimeSpan.FromMilliseconds(2000))) != null) {
                    ActiveMQMessage amqMsg = advisory as ActiveMQMessage;

                    if (amqMsg.DataStructure != null) {
                        var info = amqMsg.DataStructure as DestinationInfo;
                        if (info != null) {
                            tmpList.Add(new ActiveMQQueue(info.Destination.PhysicalName));
                        }
                    }
                }

                lock (listLock) {
                    internalQueueList ??= new List<IQueue>();
                    internalQueueList.Clear();
                    internalQueueList.AddRange(tmpList);
                }
            }
            return internalQueueList;
        }
    }
}
