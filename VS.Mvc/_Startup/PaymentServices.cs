namespace VS.Mvc._Startup {

    using Microsoft.Extensions.Configuration;
    using Serilog;
    using SimpleInjector;
    using VD.Noire;
    using VS.Abstractions.Payment;
    using VS.Core.Noire;

    public static class PaymentServices {

        public static Container AddNoire(this Container container, IConfiguration config, ILogger log) {

            var noireconfig = config.GetSection("Noire").Get<NoireOptions>();

            if (noireconfig is null) {
                log.Warning("No Noire configuration found. Skipping the Noire Client.");
                return container;
            }


            container.RegisterInstance<NoireOptions>(noireconfig);
            container.Register<INoireClient, NoireClient>();
            container.RegisterConditional<IPaymentProvider, NoirePaymentProvider>(c => !c.Handled); //TODO: Proper check

            return container;
        }
    }
}
