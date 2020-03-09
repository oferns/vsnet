namespace VS.Mvc._Startup {
    using Amazon;
    using Amazon.MQ;
    using Amazon.S3;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using SimpleInjector;
    using SimpleInjector.Diagnostics;
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


            foreach (var region in awsconfig.Regions) {
                log.Information($"Creating storage client for AWS region {region}");
                container.Collection.Append<IAmazonS3>(() => new AmazonS3Client(RegionEndpoint.GetBySystemName(region)), Lifestyle.Singleton);

                log.Information($"Creating MQ client for AWS region {region}");
                container.Collection.Append<IAmazonMQ>(() => new AmazonMQClient(RegionEndpoint.GetBySystemName(region)), Lifestyle.Singleton);
            }

            container.RegisterConditional<IStorageClient, S3StorageClient>(c => true);
            // c.Consumer.ImplementationType.FullName.StartsWith("VS.Core.Aws"));

            container.Register<IAmazonS3, RegionAwareAmazonS3Client>();
            container.Register<IAmazonMQ, RegionAwareAmazonMQClient>();

            Registration s3reg = container.GetRegistration(typeof(IAmazonS3)).Registration;
            Registration mqreq = container.GetRegistration(typeof(IAmazonMQ)).Registration;

            s3reg.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "The Amazon s3 clients should be singletons but this class just selects which of those to use on a per-request basis");
            mqreq.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "The Amazon MQ clients should be singletons but this class just selects which of those to use on a per-request basis");

            var assemblies = new[] { typeof(PutRequestDecorator).Assembly };

            var requestHandlerTypes = container.GetTypesToRegister(
                typeof(IRequestHandler<,>),
                assemblies,
                new TypesToRegisterOptions {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = true,
                    IncludeDecorators = true
                });


            foreach (var handlerType in requestHandlerTypes) {
                container.RegisterDecorator(typeof(IRequestHandler<,>), handlerType);
            }

            var requestHandlerTypes2 = container.GetTypesToRegister(
                 typeof(IRequestHandler<>),
                 assemblies,
                 new TypesToRegisterOptions {
                     IncludeGenericTypeDefinitions = true,
                     IncludeComposites = true,
                     IncludeDecorators = true
                 });

            foreach (var handlerType in requestHandlerTypes2) {
                container.RegisterDecorator(typeof(IRequestHandler<>), handlerType);
            }

            return container;
        }
    }
}