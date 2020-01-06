namespace VS.Core {
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions.Data;
    using VS.Core.Data;

    public static class ServiceCollectionExtensions {

        public static IServiceCollection AddDbServices(this IServiceCollection services) {
            return services                
                .AddScoped<IDbClient, DbClient>();
        }
    }
}