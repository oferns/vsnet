namespace VS.PostGres {
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions.Data;

    public static class ServiceCollectionExtensions {


        public static IServiceCollection AddPostGresServices(this IServiceCollection services) {
            return services                
                .Decorate<IDbClient, PgClient>();
        }
    }
}