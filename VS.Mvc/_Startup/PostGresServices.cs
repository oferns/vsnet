﻿namespace VS.Mvc._Startup {
    using System;
    using System.Data;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Schema;
    using Npgsql;
    using Serilog;
    using SimpleInjector;
    using VS.Abstractions;
    using VS.Abstractions.Data;
    using VS.Core.PostGres;
    using VS.PostGres;

    public static class PostGresServices {

        public static Container AddPostGresServices(this Container container, IConfiguration config, ILogger log) {
            if (container is null) {
                throw new ArgumentNullException(nameof(container));
            }

            if (config is null) {
                throw new ArgumentNullException(nameof(config));
            }

            if (log is null) {
                throw new ArgumentNullException(nameof(log));
            }

            var postgresconfig = config.GetSection("PostgresOptions").Get<PgOptions>();

            if (postgresconfig is null) {
                log.Warning("No Postgres configuration found. Skipping Postgres services.");
                return container;
            }

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            container.Register<IDbMessageHandler<NpgsqlNoticeEventArgs>, PgMessageHandler>();

            container.RegisterConditional<IDbClient, PgClient>(Lifestyle.Scoped, c => c.Consumer.ImplementationType.FullName.StartsWith("VS.Data.PostGres", StringComparison.InvariantCultureIgnoreCase));
            container.RegisterConditional(typeof(IQuerySyntaxProvider<>), typeof(PgSyntaxProvider<>), c => c.Consumer.ImplementationType.GenericTypeArguments[0].FullName.StartsWith("VS.Data.PostGres", StringComparison.InvariantCultureIgnoreCase));
            container.Register<IDbConnection>(() => {
                var context = container.GetInstance<IContext>();
                var connection = new NpgsqlConnection();
                return connection;
            }, Lifestyle.Scoped);

            
            container.RegisterInitializer<PgClient>(c => c.MessageRecieved += async (sender, args) => await container.GetInstance<PgMessageHandler>().MessageRecieved(sender, args).ConfigureAwait(false));

            return container;
        }

    }
}
