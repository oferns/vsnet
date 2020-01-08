namespace VS.Mvc._Startup {
    using MediatR;
    using MediatR.Pipeline;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.FileProviders;
    using SimpleInjector;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VS.Abstractions.Data;
    using VS.Abstractions.Storage;
    using VS.Core.Data;
    using VS.Core.Storage;
    using VS.Core;

    public static class AppServices {


        public static Container AddCoreServices(this Container container) {
            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);

            var coreAssemblies = new[] { typeof(AppServices).Assembly, typeof(DbClient).Assembly };

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


            container.Register<IDbClient, DbClient>();
            container.Register<IStorageClient, FileStorageClient>();
            container.Register<IFileProvider>(() => new PhysicalFileProvider(Path.GetTempPath()));
            container.RegisterSingleton<IContentTypeProvider>(()=> new  FileExtensionContentTypeProvider());
            return container;
        }

    }
}
