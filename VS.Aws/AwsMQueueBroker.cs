namespace VS.Aws {

    using Amazon.MQ;
    using Amazon.MQ.Model;
    using Amazon.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions.Messaging;

    public class AwsMQueueBroker : IQueueBroker<AmazonWebServiceResponse> {
        private readonly IAmazonMQ client;

        public AwsMQueueBroker(IAmazonMQ client) {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public AmazonWebServiceResponse Create(string queueName) {
            var res = client.CreateBrokerAsync(new CreateBrokerRequest() { BrokerName = queueName }).Result;
            return res;
        }

        public AmazonWebServiceResponse Destroy(string queueName) {
            throw new NotImplementedException();
        }

        public IEnumerable<AmazonWebServiceResponse> List(bool forceRefresh) {
            throw new NotImplementedException();
        }
    }
}
