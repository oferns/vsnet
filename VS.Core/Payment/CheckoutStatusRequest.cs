namespace VS.Core.Payment {

    using MediatR;

    public class CheckoutStatusRequest : IRequest<CheckoutStatusResponse> {

        public CheckoutStatusRequest(string checkoutId) {
            CheckoutId = checkoutId;
        }

        public string CheckoutId { get; private set; }
    }
}
