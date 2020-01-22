namespace VS.ActiveMQ.Tests {

    using Apache.NMS;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions.Messaging;

    [TestClass]
    public class TopicTests {

        [TestClass]
        public class BrokerTests {


            //[TestMethod]
            //public void ShouldListTopics() {


            //    string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
            //    NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

            //    using IConnection connection = factory.CreateConnection();
            //    connection.Start();
            //    using ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

            //    var client = new MTopicBroker(session);


            //    var list = client.List(true);

            //}

            //[TestMethod]
            //public void ShouldRemoveTopic() {

            //    string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
            //    NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

            //    using IConnection connection = factory.CreateConnection();
            //    connection.Start();
            //    using ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

            //    var client = new MTopicBroker(session);

            //    client.Destroy("test");

            //}

            //[TestMethod]
            //public void ShouldCreateTopic() {

            //    string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
            //    NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

            //    using IConnection connection = factory.CreateConnection();
            //    connection.Start();
            //    using ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

            //    var client = new MTopicBroker(session);

            //    client.Create("test");

            //}
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
                var client = new MTopicClient(session);
                var message = new Message<TestModel>(new TestModel { StringProp = "SomeString" }, "Test");
                var retmsg = client.Broadcast<TestModel>("test", message);

            }
            public class TestModel {
                public string StringProp { get; set; }
            }

        }

    }
}
