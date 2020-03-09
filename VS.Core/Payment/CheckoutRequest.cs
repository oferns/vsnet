namespace VS.Core.Payment {

    using MediatR;

    public class CheckoutRequest : IRequest<CheckoutResponse> {

        public CheckoutRequest(decimal amount, string currency, string invoiceId, string reference) {
            Amount = amount;
            Currency = currency;
            InvoiceId = invoiceId;
            Reference = reference;
        }

        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public string InvoiceId { get; private set; }
        public string Reference { get; private set; }
    }
}
                                                                                                                                        