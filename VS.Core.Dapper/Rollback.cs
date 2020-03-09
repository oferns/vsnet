namespace VS.Core.Dapper {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Data;
    using VS.Core.Data;

    public class RollbackDecorator : IRequestHandler<Rollback> {
        private readonly IRequestHandler<Rollback> wrappedClient;
        private readonly IDbClient db;

        public RollbackDecorator(IRequestHandler<Rollback> wrappedClient, IDbClient db) {
            this.wrappedClient = wrappedClient ?? throw new ArgumentNullException(nameof(wrappedClient));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<Unit> Handle(Rollback request, CancellationToken cancellationToken) {
            return new Task<Unit>(() => {
                db.Rollback();
                return default;
            });
        }
    }
}
