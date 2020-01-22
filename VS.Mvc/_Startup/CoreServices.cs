namespace VS.Mvc._Startup {
    using System;
    using System.IO;
    using System.Linq;
    using MediatR;
    using MediatR.Pipeline;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.FileProviders;
    using SimpleInjector;
    using VS.Abstractions.Data;
    using VS.Abstractions.Messaging;
    using VS.Abstractions.Storage;
    using VS.Core;
    using VS.Core.Data;
    using VS.Core.Messaging.Queue;
    using VS.Core.Storage;
    using VS.Data.PostGres.App;

    public static class CoreServices {


        public static Container AddCoreServices(this Container container) {
            
            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);

            var coreAssemblies = new[] { typeof(CoreServices).Assembly, typeof(DbClient).Assembly };

            container.RegisterSingleton<IMediator, Mediator>();

            container.GetTypesToRegister(
                typeof(IRequestHandler<,>),
                coreAssemblies,
                new TypesToRegisterOptions {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = true
                }).ToList().ForEach(t => container.Register(typeof(IRequestHandler<,>), t)); ;

            container.GetTypesToRegister(
                typeof(IRequestHandler<>),
                coreAssemblies,
                new TypesToRegisterOptions {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = true
                }).ToList().ForEach(t => container.Register(typeof(IRequestHandler<>), t));


            container.Collection.Register(typeof(IPipelineBehavior<,>), new[] {
                        typeof(RequestPreProcessorBehavior<,>),
                        typeof(RequestPostProcessorBehavior<,>)
                    });

            container.Collection.Register(typeof(IRequestPreProcessor<>), Enumerable.Empty<Type>());
            container.Collection.Register(typeof(IRequestPostProcessor<,>), Enumerable.Empty<Type>());

            container.Collection.Register(typeof(INotificationHandler<>), coreAssemblies);

            // Generic runs for alll
            container.Collection.Append(typeof(INotificationHandler<>), typeof(ChangeNotificationHandler<>));


            container.Register(typeof(IMetaData<>), typeof(Culture).Assembly);
            container.RegisterConditional<IDbClient, DbClient>(Lifestyle.Scoped, c => !c.Handled);


            container.RegisterConditional<IStorageClient, FileStorageClient>(c => !c.Handled);
            container.Register<IFileProvider>(() => new PhysicalFileProvider(Path.GetTempPath()));
            container.RegisterSingleton<IContentTypeProvider>(()=> new  FileExtensionContentTypeProvider());

            container.Register<IQueueClient, ImmediateQueueClient>();

            
            return container;
        }

    }
}
