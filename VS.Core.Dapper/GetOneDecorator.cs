namespace VS.Core.Dapper {
    
    using global::Dapper;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Data;
    using VS.Abstractions.Data.Filtering;
    using VS.Core.Data;

    public class GetOneDecorator<T> : IRequestHandler<GetOne<T>, T> where T : class {
        private readonly IRequestHandler<GetOne<T>, T> wrappedHandler;
        private readonly IDbClient db;
        private readonly IQuerySyntaxProvider<T> querySyntax;
        private readonly IFilterService<T> filter;

        public GetOneDecorator(IRequestHandler<GetOne<T>, T> wrappedHandler, IDbClient db, IQuerySyntaxProvider<T> querySyntax, IFilterService<T> filter) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.querySyntax = querySyntax ?? throw new ArgumentNullException(nameof(querySyntax));
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        public Task<T> Handle(GetOne<T> request, CancellationToken cancellationToken) {
            var sql = this.querySyntax.Select(request.Filter, request.Sorter, default, out var parameters);
            return this.db.Connection.QuerySingleOrDefaultAsync<T>(sql, parameters, db.Transaction);
        }
    }
}
