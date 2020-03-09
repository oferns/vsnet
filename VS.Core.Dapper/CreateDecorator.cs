namespace VS.Core.Dapper {

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Dapper;
    using MediatR;
    using VS.Abstractions.Data;
    using VS.Core.Data;

    public class CreateDecorator<T> : IRequestHandler<Create<T>, IEnumerable<T>> where T : class {
        private readonly IRequestHandler<Create<T>, IEnumerable<T>> wrappedHandler;
        private readonly IDbClient db;
        private readonly IQuerySyntaxProvider<T> querySyntax;

        public CreateDecorator(IRequestHandler<Create<T>, IEnumerable<T>> wrappedHandler, IDbClient db, IQuerySyntaxProvider<T> querySyntax) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.querySyntax = querySyntax ?? throw new ArgumentNullException(nameof(querySyntax));
        }

        public async Task<IEnumerable<T>> Handle(Create<T> request, CancellationToken cancellationToken) {
            var insertSql = this.querySyntax.Insert(request.Model, out var parameters);
            return await db.Connection.QueryAsync<T>(insertSql, parameters, transaction: db.Transaction);
        }
    }
}
