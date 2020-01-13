namespace VS.Core.Aws.Messaging {

    using Amazon;
    using Amazon.MQ;
    using Amazon.MQ.Model;
    using Amazon.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;

    public class RegionAwareAmazonMQClient : IAmazonMQ {
        private readonly IAmazonMQ chosenClient;
        private readonly IContext context;

        public RegionAwareAmazonMQClient(IAmazonMQ[] clients, IContext context) {
            if (clients is null) {
                throw new System.ArgumentNullException(nameof(clients));
            }
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            chosenClient = clients.Length == 0 ? new AmazonMQClient(RegionEndpoint.EUWest2) : clients[0];
        }

        public IClientConfig Config => chosenClient.Config;

        public Task<CreateBrokerResponse> CreateBrokerAsync(CreateBrokerRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.CreateBrokerAsync(request, cancellationToken);
        }

        public Task<CreateConfigurationResponse> CreateConfigurationAsync(CreateConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.CreateConfigurationAsync(request, cancellationToken);
        }

        public Task<CreateTagsResponse> CreateTagsAsync(CreateTagsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.CreateTagsAsync(request, cancellationToken);
        }

        public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.CreateUserAsync(request, cancellationToken);
        }

        public Task<DeleteBrokerResponse> DeleteBrokerAsync(DeleteBrokerRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBrokerAsync(request, cancellationToken);
        }

        public Task<DeleteTagsResponse> DeleteTagsAsync(DeleteTagsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteTagsAsync(request, cancellationToken);
        }

        public Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteUserAsync(request, cancellationToken);
        }

        public Task<DescribeBrokerResponse> DescribeBrokerAsync(DescribeBrokerRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DescribeBrokerAsync(request, cancellationToken);
        }

        public Task<DescribeBrokerEngineTypesResponse> DescribeBrokerEngineTypesAsync(DescribeBrokerEngineTypesRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DescribeBrokerEngineTypesAsync(request, cancellationToken);
        }

        public Task<DescribeBrokerInstanceOptionsResponse> DescribeBrokerInstanceOptionsAsync(DescribeBrokerInstanceOptionsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DescribeBrokerInstanceOptionsAsync(request, cancellationToken);
        }

        public Task<DescribeConfigurationResponse> DescribeConfigurationAsync(DescribeConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DescribeConfigurationAsync(request, cancellationToken);
        }

        public Task<DescribeConfigurationRevisionResponse> DescribeConfigurationRevisionAsync(DescribeConfigurationRevisionRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DescribeConfigurationRevisionAsync(request, cancellationToken);
        }

        public Task<DescribeUserResponse> DescribeUserAsync(DescribeUserRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DescribeUserAsync(request, cancellationToken);
        }

        public void Dispose() {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing) {

        }

        public Task<ListBrokersResponse> ListBrokersAsync(ListBrokersRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListBrokersAsync(request, cancellationToken);
        }

        public Task<ListConfigurationRevisionsResponse> ListConfigurationRevisionsAsync(ListConfigurationRevisionsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListConfigurationRevisionsAsync(request, cancellationToken);
        }

        public Task<ListConfigurationsResponse> ListConfigurationsAsync(ListConfigurationsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListConfigurationsAsync(request, cancellationToken);
        }

        public Task<ListTagsResponse> ListTagsAsync(ListTagsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListTagsAsync(request, cancellationToken);
        }

        public Task<ListUsersResponse> ListUsersAsync(ListUsersRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListUsersAsync(request, cancellationToken);
        }

        public Task<RebootBrokerResponse> RebootBrokerAsync(RebootBrokerRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.RebootBrokerAsync(request, cancellationToken);
        }

        public Task<UpdateBrokerResponse> UpdateBrokerAsync(UpdateBrokerRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.UpdateBrokerAsync(request, cancellationToken);
        }

        public Task<UpdateConfigurationResponse> UpdateConfigurationAsync(UpdateConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.UpdateConfigurationAsync(request, cancellationToken);
        }

        public Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.UpdateUserAsync(request, cancellationToken);
        }
    }
}
