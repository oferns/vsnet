namespace VS.Mvc._Startup {
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using SimpleInjector;
    using VD.PayOn;
    using VS.Core.PayOn;

    public static class PaymentServices {

        public static Container AddPayOn(this Container container, IConfiguration config, ILogger log) {

            var PayOnconfig = config.GetSection("PayOn").Get<PayOnOptions>();

            if (PayOnconfig is null) {
                log.Warning("No PayOn configuration found. Skipping the PayOn Client.");
                return container;
            }


            var requestHandlerTypes = container.GetTypesToRegister(
                typeof(IRequestHandler<,>),
                 new[] { typeof(CheckoutDecorator).Assembly },
                new TypesToRegisterOptions {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = true,
                    IncludeDecorators = true
                });


            var cacheDecorators = new List<Type>();

            foreach (var handlerType in requestHandlerTypes) {
                if (handlerType.Name.StartsWith("Cache")) {
                    cacheDecorators.Add(handlerType);
                } else {
                    container.RegisterDecorator(typeof(IRequestHandler<,>), handlerType);
                }
            }

            foreach (var decoratorType in cacheDecorators) {
                container.RegisterDecorator(typeof(IRequestHandler<,>), decoratorType);
            }


            var requestHandlerTypes2 = container.GetTypesToRegister(
               typeof(IRequestHandler<>),
               new[] { typeof(CheckoutDecorator).Assembly },
               new TypesToRegisterOptions {
                   IncludeGenericTypeDefinitions = true,
                   IncludeComposites = true,
                   IncludeDecorators = true
               });

            foreach (var handlerType in requestHandlerTypes2) {
                if (handlerType.Name.StartsWith("Cache")) {
                    cacheDecorators.Add(handlerType);
                } else {
                    container.RegisterDecorator(typeof(IRequestHandler<,>), handlerType);
                }
            }

            foreach (var decoratorType in cacheDecorators) {
                container.RegisterDecorator(typeof(IRequestHandler<,>), decoratorType);
            }

            container.RegisterInstance<PayOnOptions>(PayOnconfig);
            container.Register<IPayOnClient, PayOnClient>();

            return container;
        }
    }
}
