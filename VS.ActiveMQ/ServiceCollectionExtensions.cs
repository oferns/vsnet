namespace VS.ActiveMQ {
    using Apache.NMS;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using VS.Abstractions.Messaging;

    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddActiveMQServices(this IServiceCollection services, MQueueOptions options) {
            if (services is null) {
                throw new ArgumentNullException(nameof(services));
            }

            if (options is null) {
                throw new ArgumentNullException(nameof(options));
            }

            return services
                .AddSingleton<IConnectionFactory>(new NMSConnectionFactory(options.Address))
                .AddScoped<IConnection>(s => s.GetService<IConnectionFactory>().CreateConnection())
                .AddScoped<ISession>(s => {
                    var con = s.GetService<IConnection>();
                    con.Start();
                    return con.CreateSession();
                })
                .AddScoped<IQueueBroker<IQueue>, MQueueBroker>()
                .AddScoped<IQueueClient<IQueue>, MQueueClient>()
                .AddScoped<ITopicBroker<ITopic>, MTopicBroker>()
                .AddScoped<ITopicClient<ITopic>, MTopicClient>();
        }
    }
}