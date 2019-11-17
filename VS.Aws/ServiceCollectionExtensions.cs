﻿namespace VS.Aws {
    using Amazon.MQ;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions.Queues;
    using VS.Abstractions.Storage;

    public static class ServiceCollectionExtensions {

        public static IServiceCollection AddAWSServices(this IServiceCollection services) {
            return services
                    .AddSingleton<IAmazonMQ,AmazonMQClient>()
                    .AddTransient<IStorageClient, S3StorageClient>()
                    .AddTransient<IQueueClient, MqQueueClient>();
        }
    }
}
