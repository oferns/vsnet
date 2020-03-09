namespace VS.Core.Payment.Handlers {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Data.Paging;

    public class RegisteredCardsHandler : IRequestHandler<RegisteredCardsRequest, PagedList<RegisteredCard>> {

        public Task<PagedList<RegisteredCard>> Handle(RegisteredCardsRequest request, CancellationToken cancellationToken) {
            return Task.FromResult(new PagedList<RegisteredCard>(Array.Empty<RegisteredCard>(), 0, 0, 0));
        }

    }
}
