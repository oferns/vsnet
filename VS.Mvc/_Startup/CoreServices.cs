namespace VS.Mvc._Startup {
    using System;
    using System.IO;
#if DEBUG
    using AutoBogus;
#endif
    using MediatR;
    using MediatR.Pipeline;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.FileProviders;
    using SimpleInjector;
    using VS.Abstractions;
    using VS.Abstractions.Data;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Data.Paging;
    using VS.Abstractions.Data.Sorting;
    using VS.Abstractions.Messaging;
    using VS.Abstractions.Storage;
    using VS.Core;
    using VS.Core.Local.Storage;
    using VS.Core.Messaging.Queue;
    using VS.Core.Storage.Handlers;
    using VS.Data.PostGres.App.Meta;
    using VS.Mvc._Services;

    public static class CoreServices {


        public static Container AddCoreServices(this Container container) {

            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);

            var coreAssemblies = new[] { typeof(CoreServices).Assembly, typeof(GetIndexHandler).Assembly };

            container.RegisterSingleton<IMediator, Mediator>();

            var coreRequestHandlers = container.GetTypesToRegister(
                typeof(IRequestHandler<,>),
                coreAssemblies,
                new TypesToRegisterOptions {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = true
                });


            foreach (var handler in coreRequestHandlers) {
                container.Register(typeof(IRequestHandler<,>), handler);
            }

            var coreRequestHandlers2 = container.GetTypesToRegister(
                typeof(IRequestHandler<>),
                coreAssemblies,
                new TypesToRegisterOptions {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = true
                });

            foreach (var handler in coreRequestHandlers2) {
                container.Register(typeof(IRequestHandler<>), handler);
            }

            container.Collection.Register(typeof(IPipelineBehavior<,>), new[] {
                        typeof(RequestPreProcessorBehavior<,>),
                        typeof(RequestPostProcessorBehavior<,>)
                    });

            container.Collection.Register(typeof(IRequestPreProcessor<>), Array.Empty<Type>());
            container.Collection.Register(typeof(IRequestPostProcessor<,>), Array.Empty<Type>());

            container.Collection.Register(typeof(INotificationHandler<>), coreAssemblies);
            
            container.Collection.Append(typeof(INotificationHandler<>), typeof(ChangeNotificationHandler<>));


            container.Register(typeof(IMetaData<>), typeof(CultureMetadata).Assembly);
            container.RegisterConditional<IDbClient, DbClient>(Lifestyle.Scoped, c => !c.Handled);


            container.RegisterConditional<IStorageClient, FileStorageClient>(c => !c.Handled);
            container.Register<IFileProvider>(() => new PhysicalFileProvider(Path.GetTempPath()));
            container.RegisterSingleton<IContentTypeProvider>(() => new FileExtensionContentTypeProvider());

            container.Register<IQueueClient, ImmediateQueueClient>();

            container.Register<IPager, QueryStringPager>();
            container.Register(typeof(IFilterService<>), typeof(QueryStringFilterService<>));
            container.Register(typeof(ISorterService<>), typeof(QueryStringSorterService<>));
            container.Register<IFlashMessager, HttpFlashMessager>();          

            // Fake Data Handlers
#if DEBUG

            container.RegisterSingleton(typeof(IAutoFaker), () => AutoFaker.Create());

            var requestHandlerTypes = container.GetTypesToRegister(
                typeof(IRequestHandler<,>),
                 new[] { typeof(Core.FakeData.Data.GetOneDecorator<>).Assembly },
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
                new[] { typeof(Core.FakeData.Data.GetOneDecorator<>).Assembly },
                new TypesToRegisterOptions {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = true,
                    IncludeDecorators = true
                });

            foreach (var handlerType in requestHandlerTypes2) {
                container.RegisterDecorator(typeof(IRequestHandler<>), handlerType);
            }
#endif

            return container;
        }

    }
}
