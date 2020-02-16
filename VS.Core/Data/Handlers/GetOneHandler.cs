namespace VS.Core.Data.Handlers {

    using Dapper;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Data;

    public class GetOneHandler<T> : IRequestHandler<GetOne<T>, T> where T : class {
        private readonly IDbClient db;
        private readonly IQuerySyntaxProvider<T> querySyntax;

        public GetOneHandler(IDbClient db, IQuerySyntaxProvider<T> querySyntax) {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.querySyntax = querySyntax ?? throw new ArgumentNullException(nameof(querySyntax));
        }

        public Task<T> Handle(GetOne<T> request, CancellationToken cancellationToken) {
            var sql = this.querySyntax.Select(request.Filter, request.Sorter, default, out var parameters);
            return this.db.Connection.QuerySingleOrDefaultAsync<T>(sql, parameters, db.Transaction);
        }
    }
}
