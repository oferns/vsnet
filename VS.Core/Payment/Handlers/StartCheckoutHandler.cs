namespace VS.Core.Payment.Handlers {
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Payment;

    public class StartCheckoutHandler : IRequestHandler<StartCheckoutRequest, CheckoutResponse> {
        private readonly IPaymentProvider provider;

        public StartCheckoutHandler(IPaymentProvider provider) {
            this.provider = provider;
        }


        public async Task<CheckoutResponse> Handle(StartCheckoutRequest request, CancellationToken cancel) {
            return await provider.Checkout(new CheckoutRequest(request.Amount, request.Currency, request.InvoiceId, request.Reference), cancel);
        }
    }
}
