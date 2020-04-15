namespace VS.Mvc._Startup {

    using MessagePack;
    using MessagePack.Resolvers;
    using SimpleInjector;
    using VS.Abstractions;
    using VS.MPack;

    public static class SerializationConfig {

        public static Container AddSerializationServices(this Container container) {
            if (container is null)
            {
                throw new System.ArgumentNullException(nameof(container));
            }

            container.RegisterSingleton<MessagePackSerializerOptions>(
                () => MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolverAllowPrivate.Instance)
                );
            container.RegisterSingleton<ISerializer, MPSerializer>();
            return container;
        }

    }
}
