namespace VS.Core.Data.Handlers {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Data.Paging;

    public class GetListHandler<T> : IRequestHandler<GetList<T>, PagedList<T>> where T : class {
        public Task<PagedList<T>> Handle(GetList<T> request, CancellationToken cancel) {
            return Task.FromResult(new PagedList<T>(Array.Empty<T>(), 0, 10, 0));
        }
    }

    public class GetListHandler<F, T> : IRequestHandler<GetList<F, T>, PagedList<T>> where F : class where T : class {
        public Task<PagedList<T>> Handle(GetList<F, T> request, CancellationToken cancellationToken) {

            return Task.FromResult(new PagedList<T>(Array.Empty<T>(), 0, 10, 0));

        }
    }

}
