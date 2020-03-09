namespace VS.Mvc._Startup {

    using MessagePack;
    using MessagePack.Resolvers;
    using SimpleInjector;
    using VS.Abstractions;
    using VS.MPack;

    public static class Serialization {

        public static Container AddSerializationServices(this Container container) {
            container.RegisterSingleton<MessagePackSerializerOptions>(
                () => MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolverAllowPrivate.Instance)
                );
            container.RegisterSingleton<ISerializer, MPSerializer>();
            return container;
        }

    }
}
