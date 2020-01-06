namespace VS.ActiveMQ.Tests {
    using Apache.NMS;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions.Messaging;

    [TestClass]
    public class QueueTests {

        [TestClass]
        public class BrokerTests {


            [TestMethod]
            public void ShouldListQueues() {


                string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
                NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

                using IConnection connection = factory.CreateConnection();
                connection.Start();
                using ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

                var client = new MQueueBroker(session);


                var list = client.List(true);

            }

            [TestMethod]
            public void ShouldRemoveQueue() {

                string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
                NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

                using IConnection connection = factory.CreateConnection();
                connection.Start();
                using ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

                var client = new MQueueBroker(session);

                client.Destroy("test");

            }

            [TestMethod]
            public void ShouldCreateQueue() {

                string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
                NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

                using IConnection connection = factory.CreateConnection();
                connection.Start();
                using ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

                var client = new MQueueBroker(session);

                client.Create("test");

            }
        }
        

        [TestClass]
        public class ClientTests {


            [TestMethod]
            public void ShouldSendMessage() {
                string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
                NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);
                using IConnection connection = factory.CreateConnection();
                connection.Start();
                using ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                var client = new MQueueClient(session);
                var message = new Message<TestModel>(new TestModel { StringProp = "SomeString" }, "Test");
                var retmsg = client.Send<TestModel>("test", message);

            }
            public class TestModel { 
                public string StringProp { get; set; }
            }

        }
    }
}