namespace VS.Core.Dapper {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Data;
    using VS.Core.Data;

    public class BeginDecorator : IRequestHandler<Begin> {
        private readonly IRequestHandler<Begin> wrappedHandler;
        private readonly IDbClient db;

        public BeginDecorator(IRequestHandler<Begin> wrappedHandler, IDbClient db) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<Unit> Handle(Begin request, CancellationToken cancellationToken) {
            return new Task<Unit>(() => {
                db.BeginTransaction();
                return default;
            });
        }
    }
}
