namespace VS.Core.Payment {
    using MediatR;
    using VS.Abstractions.Data.Paging;

    public class RegisteredCardsRequest : IRequest<PagedList<RegisteredCard>> {

        public RegisteredCardsRequest(string userId) {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }
}