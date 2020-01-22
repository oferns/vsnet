namespace VS.Aws {

    using Amazon.MQ;
    using Amazon.MQ.Model;
    using Amazon.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Messaging;

    public class AwsMessageBroker : IMessageBroker<BrokerSummary> {
        
        private readonly IAmazonMQ client;

        public AwsMessageBroker(IAmazonMQ client) {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<BrokerSummary> Create(string brokerName, CancellationToken cancel) {
            var broker = await client.CreateBrokerAsync(new CreateBrokerRequest {
                BrokerName = brokerName
            }, cancel);

            var brokerdesc = await client.DescribeBrokerAsync(new DescribeBrokerRequest() { BrokerId = broker.BrokerId });

            return new BrokerSummary {
                BrokerId = brokerdesc.BrokerId,
                BrokerArn = brokerdesc.BrokerArn,
                BrokerName = brokerdesc.BrokerName,
                BrokerState = brokerdesc.BrokerState,
                Created = brokerdesc.Created,
                DeploymentMode = brokerdesc.DeploymentMode,
                HostInstanceType = brokerdesc.HostInstanceType
            };
        }

        public async Task<BrokerSummary> Destroy(string brokerId, CancellationToken cancel) {
            var brokerdesc = await client.DescribeBrokerAsync(
                new DescribeBrokerRequest {
                    BrokerId = brokerId
                }, cancel);

            var response = await client.DeleteBrokerAsync(new DeleteBrokerRequest { BrokerId = brokerdesc.BrokerId }, cancel);

            return new BrokerSummary {
                BrokerId = response.BrokerId,
                BrokerArn = brokerdesc.BrokerArn,
                BrokerName = brokerdesc.BrokerName,
                BrokerState = BrokerState.DELETION_IN_PROGRESS,
                Created = brokerdesc.Created,
                DeploymentMode = brokerdesc.DeploymentMode,
                HostInstanceType = brokerdesc.HostInstanceType
            };
        }

        public async Task<PagedBrokerIndex<BrokerSummary>> List(int pageSize, string token, CancellationToken cancel) {
            var result = await client.ListBrokersAsync(new ListBrokersRequest {
                MaxResults = pageSize,
                NextToken = token
            }, cancel);

            return new PagedBrokerIndex<BrokerSummary>(result.BrokerSummaries, pageSize, token);

        }
    }
}
