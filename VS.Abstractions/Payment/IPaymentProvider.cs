namespace VS.Abstractions.Payment {
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPaymentProvider {

        Task<CheckoutResponse> Checkout(CheckoutRequest request, CancellationToken cancel);

        Task<CheckoutStatusResponse> CheckoutStatus(CheckoutStatusRequest request, CancellationToken cancel);

        Task<CheckoutCompleteResponse> CompleteCheckout(CheckoutCompleteRequest request, CancellationToken cancel);

    }
}
