namespace VS.Core.PayOn {

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Data.Paging;
    using VS.Core.Payment;
    using VS.Core.Storage;

    public class RegisteredCardsListDecorator : IRequestHandler<RegisteredCardsRequest, PagedList<RegisteredCard>> {
        
        private readonly IMediator mediator;

        public RegisteredCardsListDecorator(IRequestHandler<RegisteredCardsRequest, PagedList<RegisteredCard>> wrappedHandler, IMediator mediator) {
            _ = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PagedList<RegisteredCard>> Handle(RegisteredCardsRequest request, CancellationToken cancellationToken) {

            var cardList = await mediator.Send(new GetIndex(new Uri($"vs-cards/{request.UserId}", UriKind.Relative), 5, null));

            var list = new List<RegisteredCard>();
                                   
            foreach (var card in cardList) {
                list.Add(await mediator.Send(new GetObject<RegisteredCard>(card.Location)));                
            }

            return new PagedList<RegisteredCard>(list, 0, 5, list.Count);
        }
    }
}
