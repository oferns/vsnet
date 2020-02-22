
namespace VS.Core.Payment {
    using MediatR;
    using VS.Abstractions.Payment;

    public class StartCheckoutRequest : IRequest<CheckoutResponse> {

        public StartCheckoutRequest(decimal amount, string currency, string invoiceId, string reference) {
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
