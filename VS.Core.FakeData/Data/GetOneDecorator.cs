namespace VS.Core.FakeData.Data {

    using AutoBogus;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Abstractions.Data.Filtering;
    using VS.Core.Data;

    public class GetOneDecorator<T> : IRequestHandler<GetOne<T>, T> where T : class {

        private readonly IRequestHandler<GetOne<T>, T> wrappedHandler;
        private readonly IContext context;
        private readonly IAutoFaker faker;
        private readonly IFilterService<T> filterService;

        public GetOneDecorator(IRequestHandler<GetOne<T>, T> wrappedHandler, IContext context, IAutoFaker faker, IFilterService<T> filterService) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.faker = faker ?? throw new ArgumentNullException(nameof(faker));
            this.filterService = filterService ?? throw new ArgumentNullException(nameof(filterService));
        }

        public async Task<T> Handle(GetOne<T> request, CancellationToken cancel) {

            if (context is object) {         // This is always true
                var obj = this.faker.Generate<T>(config => config.WithLocale(context.UICulture.Name.Replace('-', '_')));

                foreach (var entry in filterService.Filter) {
                    if (entry.Clause is Clause<T> singleclause) {
                        singleclause.Property.SetValue(obj, singleclause.Value);
                    }
                }

                return obj;
            }

            return await wrappedHandler.Handle(request, cancel);
        }
    }
}
