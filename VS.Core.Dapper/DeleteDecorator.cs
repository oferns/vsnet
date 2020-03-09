namespace VS.Core.Dapper {

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Dapper;
    using MediatR;
    using VS.Abstractions.Data;
    using VS.Abstractions.Data.Filtering;
    using VS.Core.Data;

    public class DeleteDecorator<T> : IRequestHandler<Delete<T>, IEnumerable<T>> where T : class {

        private readonly IRequestHandler<Delete<T>, IEnumerable<T>> wrappedHandler;
        private readonly IDbClient db;
        private readonly IQuerySyntaxProvider<T> querySyntax;
        private readonly IFilterService<T> filter;

        public DeleteDecorator(IRequestHandler<Delete<T>, IEnumerable<T>> wrappedHandler, IDbClient db, IQuerySyntaxProvider<T> querySyntax, IFilterService<T> filter) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.querySyntax = querySyntax ?? throw new ArgumentNullException(nameof(querySyntax));
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        public async Task<IEnumerable<T>> Handle(Delete<T> request, CancellationToken cancellationToken) {
            var insertSql = querySyntax.Delete(filter.Filter, out var parameters);
            return await db.Connection.QueryAsync<T>(insertSql, parameters, transaction: db.Transaction);
        }
    }
}