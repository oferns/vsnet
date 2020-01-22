namespace VS.Mvc._Startup {
    
    using MessagePack;
    using MessagePack.Resolvers;
    using SimpleInjector;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.MPack;

    public static class Serialization {

        public static Container AddSerializationServices(this Container container) {
            container.RegisterSingleton<MessagePackSerializerOptions>(
                () => MessagePackSerializerOptions.Standard.WithResolver(TypelessContractlessStandardResolver.Instance)
                );
            container.RegisterSingleton<ISerializer, MPSerializer>();
            return container;
        }

    }
}
