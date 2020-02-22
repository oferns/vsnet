
namespace VS.Core.Noire {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VD.Noire;
    using VS.Abstractions;
    using VS.Abstractions.Payment;

    public class NoirePaymentProvider : IPaymentProvider {
        private readonly INoireClient client;
        private readonly IContext context;

        public NoirePaymentProvider(INoireClient client, IContext context) {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<CheckoutResponse> Checkout(CheckoutRequest request, CancellationToken cancel) {

            var checkout = new Checkout(
                    request.Amount,
                    request.Currency,
                    PaymentType.Debit,
                    merchantTransactionId: request.Reference,
                    merchantInvoiceId: request.InvoiceId,
                    descriptor: context.Host,
                    transactionCategory: TransactionCategory.eCommerce
                );

            var noireResponse = await client.CreateCheckout(checkout, cancel);

            var obj = new CheckoutResponse(noireResponse.Result.IsSuccess(), noireResponse.Id, noireResponse.Timestamp);

            return obj;
        }

        public async Task<CheckoutStatusResponse> CheckoutStatus(CheckoutStatusRequest request, CancellationToken cancel) {

            var noireresponse = await client.CheckoutStatus(request.ProviderIdentifier, cancel);
            var obj = new CheckoutStatusResponse();

            return obj;

        }

        public Task<CheckoutCompleteResponse> CompleteCheckout(CheckoutCompleteRequest request, CancellationToken cancel) {
            throw new NotImplementedException();
        }
    }
}
