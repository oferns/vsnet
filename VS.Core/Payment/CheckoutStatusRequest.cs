namespace VS.Core.Payment {
    using MediatR;

    public class CheckoutStatusRequest : IRequest<CheckoutStatusResponse> {

        public CheckoutStatusRequest(string orderId, string providerId, bool refresh = false) {
            OrderId = orderId;
            ProviderId = providerId;
            Refresh = refresh;
        }

        public string OrderId { get; private set; }
        public string ProviderId { get; private set; }
        public bool Refresh { get; }
    }
}
