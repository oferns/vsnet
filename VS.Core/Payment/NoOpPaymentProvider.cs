namespace VS.Core.Payment {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Abstractions.Payment;


    public class NoOpPaymentProvider : IPaymentProvider {
        private readonly IFlashMessager flash;

        public NoOpPaymentProvider(IFlashMessager flash) {
            this.flash = flash ?? throw new ArgumentNullException(nameof(flash));
        }


        public Task<CheckoutResponse> Checkout(CheckoutRequest request, CancellationToken cancel) {
            flash.Flash(FlashLevel.Failed, "No payment provider is available.");
            return Task.FromResult(new CheckoutResponse(false, string.Empty, DateTimeOffset.Now));
        }

        public Task<CheckoutStatusResponse> CheckoutStatus(CheckoutStatusRequest request, CancellationToken cancel) {
            flash.Flash(FlashLevel.Failed, "No payment provider is available.");
            return Task.FromResult(new CheckoutStatusResponse());

        }

        public Task<CheckoutCompleteResponse> CompleteCheckout(CheckoutCompleteRequest request, CancellationToken cancel) {
            flash.Flash(FlashLevel.Failed, "No payment provider is available.");
            return Task.FromResult(new CheckoutCompleteResponse());
        }
    }
}
