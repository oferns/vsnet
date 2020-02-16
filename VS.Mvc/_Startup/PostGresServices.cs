namespace VS.Mvc._Startup {
    using Npgsql;
    using SimpleInjector;
    using VS.Abstractions.Data;
    using VS.Core.PostGres;
    using VS.PostGres;

    public static class PostGresServices {

        public static Container AddPostGresServices(this Container container) {

            container.Register<IDbMessageHandler<NpgsqlNoticeEventArgs>, PgMessageHandler>();

            container.RegisterConditional<IDbClient, PgClient>(Lifestyle.Scoped, c => c.Consumer.ImplementationType.FullName.StartsWith("VS.Data.PostGres"));
            container.RegisterConditional(typeof(IQuerySyntaxProvider<>), typeof(PgSyntaxProvider<>), c => c.Consumer.ImplementationType.GenericTypeArguments[0].FullName.StartsWith("VS.Data.PostGres"));

            container.Register(typeof(IQuerySyntaxProvider<>),typeof(PgClient).Assembly);
            container.RegisterInitializer<PgClient>(c => c.MessageRecieved += async (o, e) => await container.GetInstance<PgMessageHandler>().MessageRecieved(o, e));

            return container;
        }

    }
}
