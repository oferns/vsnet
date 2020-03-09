namespace VS.Core.Dapper {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Data;
    using VS.Core.Data;

    public class CommitDecorator : IRequestHandler<Commit> {
        private readonly IRequestHandler<Commit> wrappedHandler;
        private readonly IDbClient db;

        public CommitDecorator(IRequestHandler<Commit> wrappedHandler, IDbClient db) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<Unit> Handle(Commit request, CancellationToken cancellationToken) {
            return new Task<Unit>(() => {
                db.Commit();
                return default;
            });
        }        
    }
}
