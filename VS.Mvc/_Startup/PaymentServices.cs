namespace VS.Mvc._Startup {
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

            foreach (var handlerType in requestHandlerTypes) {
                container.RegisterDecorator(typeof(IRequestHandler<,>), handlerType);
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
                container.RegisterDecorator(typeof(IRequestHandler<>), handlerType);
            }

            container.RegisterInstance<PayOnOptions>(PayOnconfig);
            container.Register<IPayOnClient, PayOnClient>();

            return container;
        }
    }
}
