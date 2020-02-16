namespace VS.Core.Data.Handlers {

    using Dapper;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Data;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Data.Paging;
    using VS.Abstractions.Data.Sorting;

    public class GetListHandler<T> : IRequestHandler<GetList<T>, PagedList<T>> where T : class {
        private readonly IDbClient db;
        private readonly IQuerySyntaxProvider<T> querySyntax;
        private readonly IFilterService<T> filter;
        private readonly ISorterService<T> sorter;
        private readonly IPager pager;

        public GetListHandler(IDbClient db, IQuerySyntaxProvider<T> querySyntax, IFilterService<T> filter, ISorterService<T> sorter, IPager pager) {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.querySyntax = querySyntax ?? throw new ArgumentNullException(nameof(querySyntax));
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
            this.sorter = sorter ?? throw new ArgumentNullException(nameof(sorter));
            this.pager = pager ?? throw new ArgumentNullException(nameof(pager));
        }

        public async Task<PagedList<T>> Handle(GetList<T> request, CancellationToken cancel) {

            var queryfilter = request.Filter ?? filter.GetFilter();
            var querySorter = request.Sorter ?? sorter.GetSorter();
                    
            var sql = this.querySyntax.Select(queryfilter, querySorter, pager, out var parameters);           
            var countSql = this.querySyntax.Count(queryfilter, out _);

            var list = await db.Connection.QueryAsync<T>(sql, parameters, db.Transaction);
            var count = await db.Connection.QueryFirstAsync<int>(countSql, parameters, db.Transaction);

            return new PagedList<T>(list, 0, 10, count);
            
        }
    }

    public class GetListHandler<F, T> : IRequestHandler<GetList<F, T>, PagedList<T>> where F : class where T : class {
        private readonly IDbClient db;
        private readonly IQuerySyntaxProvider<T> querySyntax;
        private readonly IFilterService<T> filter;
        private readonly ISorterService<T> sorter;
        private readonly IPager pager;

        public GetListHandler(IDbClient db, IQuerySyntaxProvider<T> querySyntax, IFilterService<T> filter, ISorterService<T> sorter, IPager pager) {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.querySyntax = querySyntax ?? throw new ArgumentNullException(nameof(querySyntax));
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
            this.sorter = sorter ?? throw new ArgumentNullException(nameof(sorter));
            this.pager = pager ?? throw new ArgumentNullException(nameof(pager));
        }

        public async Task<PagedList<T>> Handle(GetList<F, T> request, CancellationToken cancellationToken) {

            var queryfilter = request.Filter ?? filter.GetFilter();
            var querySorter = request.Sorter ?? sorter.GetSorter();

            var sql = this.querySyntax.Select<F>(request.Function, queryfilter, querySorter, pager, out var parameters);
            var countSql = this.querySyntax.Count<F>(request.Function, queryfilter, out _);

            var list = await db.Connection.QueryAsync<T>(sql, parameters, db.Transaction);
            var count = await db.Connection.QueryFirstAsync<int>(countSql, parameters, db.Transaction);

            return new PagedList<T>(list, 0, 10, count);

        } 
    }

}
