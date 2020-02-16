namespace VS.Core.FakeData.Data {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoBogus;
    using MediatR;
    using VS.Abstractions;
    using VS.Abstractions.Data.Paging;
    using VS.Core.Data;

    public class GetListDecorator<T> : IRequestHandler<GetList<T>, PagedList<T>> where T : class {
        
        private readonly IRequestHandler<GetList<T>, PagedList<T>> wrappedHandler;
        private readonly IContext context;
        private readonly IAutoFaker faker;

        public GetListDecorator(IRequestHandler<GetList<T>, PagedList<T>> wrappedHandler, IContext context, IAutoFaker faker) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.faker = faker ?? throw new ArgumentNullException(nameof(faker));
        }


        public Task<PagedList<T>> Handle(GetList<T> request, CancellationToken cancellationToken) {

            if (true) {
                var pager = request.Pager ?? new Pager(0, 25);
                var list = faker.Generate<T>(pager.PageSize, config => config.WithLocale(context.UICulture.Name.Replace('-', '_')));
                return Task.FromResult(new PagedList<T>(list, pager.StartFrom, pager.PageSize, 200));
            }
            return wrappedHandler.Handle(request, cancellationToken);
        }
    }
}