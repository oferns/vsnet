namespace VS.Mvc._Startup {
    using Amazon;
    using Amazon.MQ;
    using Amazon.S3;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using SimpleInjector;
    using System.Linq;
    using VS.Abstractions.Storage;
    using VS.Aws;
    using VS.Core.Aws.Messaging;
    using VS.Core.Aws.Storage;

    public static class AwsServices {

        public static Container AddAwsServices(this Container container, IConfiguration config, ILogger log) {

            var awsconfig = config.GetSection("AwsConfig").Get<AwsConfig>();

            if (awsconfig is null) {
                log.Warning("No AWS configuration found. Skipping AWS services.");
                return container;
            }

            container.Collection.Register<IAmazonS3>(awsconfig.Regions.Select(r => {
                log.Information($"Creating storage client for AWS region {r}");
                return new AmazonS3Client(RegionEndpoint.GetBySystemName(r)); 
            }).ToArray());

            container.Register<IAmazonS3, RegionAwareAmazonS3Client>();
            container.RegisterConditional<IStorageClient, S3StorageClient>(c => c.Consumer.ImplementationType.FullName.StartsWith("VS.Core.Aws"));

            container.Collection.Register<IAmazonMQ>(awsconfig.Regions.Select(r => {
                log.Information($"Creating MQ client for AWS region {r}");
                return new AmazonMQClient(RegionEndpoint.GetBySystemName(r));
            }).ToArray());

            container.Register<IAmazonMQ, RegionAwareAmazonMQClient>();

            var assemblies = new[] { typeof(PutRequestDecorator).Assembly }; 

            container.GetTypesToRegister(
                typeof(IRequestHandler<,>),
                assemblies,
                new TypesToRegisterOptions {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = true,
                    IncludeDecorators = true
                }).ToList()
                .ForEach(t => container.RegisterDecorator(typeof(IRequestHandler<,>), t));

            container.GetTypesToRegister(
                typeof(IRequestHandler<>),
                assemblies,
                new TypesToRegisterOptions {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = true,
                    IncludeDecorators = true
                }).ToList()
                .ForEach(t => container.RegisterDecorator(typeof(IRequestHandler<>), t));


            return container;
        }
    }
}