namespace VS.Core.Payment {

    using MediatR;

    public class CheckoutRequest : IRequest<CheckoutResponse> {

        public CheckoutRequest(string orderId, decimal amount, string currency, string invoiceId) {
            OrderId = orderId;
            Amount = amount;
            Currency = currency;
            InvoiceId = invoiceId;
        }

        public string OrderId { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }
        public string InvoiceId { get; private set; }
    }
}
                                                                                                                                        