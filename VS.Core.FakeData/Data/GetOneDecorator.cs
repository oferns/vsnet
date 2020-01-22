namespace VS.Core.FakeData.Data {

    using AutoBogus;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Core.Data;

    public class GetOneDecorator<T> : IRequestHandler<GetOne<T>, T> where T : class {

        private readonly GetOneDecorator<T> wrappedHandler;
        private readonly IContext context;
        private readonly IAutoFaker faker;

        public GetOneDecorator(GetOneDecorator<T> wrappedHandler, IContext context, IAutoFaker faker) {
            this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.faker = faker ?? throw new ArgumentNullException(nameof(faker));
        }

        public async Task<T> Handle(GetOne<T> request, CancellationToken cancel) {

            if (context.User.HasClaim(c => c.Type.Equals("vsfake") && c.Value.Equals("true"))) {
                return this.faker.Generate<T>(config => config.WithLocale(context.UICulture.Name));
            }

            return await wrappedHandler.Handle(request, cancel);
        }
    }
}
