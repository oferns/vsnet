namespace VS.Core.PayOn {

    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VD.PayOn;
    using VS.Abstractions;
    using VS.Core.Identity;
    using VS.Core.Payment;

    public class CheckoutDecorator : IRequestHandler<CheckoutRequest, CheckoutResponse> {
        
        private readonly IRequestHandler<CheckoutRequest, CheckoutResponse> wrappedClient;
        private readonly IPayOnClient client;
        private readonly IContext context;

        public CheckoutDecorator(
                IRequestHandler<CheckoutRequest, CheckoutResponse> wrappedClient,
                IPayOnClient client, 
                IContext context) {
            this.wrappedClient = wrappedClient ?? throw new ArgumentNullException(nameof(wrappedClient));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancellationToken) {
            var res = await wrappedClient.Handle(request, cancellationToken);
            
            if (res is object) {
                return res;
            }

            var cards = context.User.FindAll(IdClaimTypes.CardRef + "/PayOn");

            var list = new List<string>();
            var en = cards.GetEnumerator();

            while (en.MoveNext()) {
                list.Add(en.Current.Value);
            }


            var checkout = new Checkout(
                   request.Amount,
                   request.Currency,
                   PaymentType.Debit,
                   registrationIds: list,
                   merchantTransactionId: request.OrderId,
                   merchantInvoiceId: request.InvoiceId,
                   descriptor: context.Host,
                   transactionCategory: TransactionCategory.eCommerce
               );

            var PayOnResponse = await client.CreateCheckout(checkout, cancellationToken);


            var obj = new CheckoutResponse(                
                    PayOnResponse.Result.DidntFail(),
                    PayOnResponse.Id, 
                    PayOnResponse.Timestamp
                );

            return obj;
        }
    }
}
