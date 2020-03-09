namespace VD.PayOn {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VD.PayOn.BackOffice;

    public interface IPayOnClient {
        
        Uri BaseUri { get; }

        Task<PayOnResponse> CheckoutStatus(string resourcePath, CancellationToken cancel);
        Task<PayOnResponse> CreateCheckout(Checkout request, CancellationToken cancel);
        Task<PayOnResponse> RegisterCard(RegisterCard registerCard, CancellationToken cancel);
        Task<RegisterCardResponse> RegisterCardStatus(string registrationId, CancellationToken cancel);
        Task<PayOnResponse> DeRegisterCard(DeRegisterCard registerCard, CancellationToken cancel);

        Task<PayOnResponse> Capture(Capture capture, CancellationToken cancel);
        Task<PayOnResponse> Refund(Refund refund, CancellationToken cancel);
        Task<PayOnResponse> Credit(Credit credit, CancellationToken cancel);
        Task<PayOnResponse> Rebill(Rebill rebill, CancellationToken cancel);
        Task<PayOnResponse> Reversal(Reversal reversal, CancellationToken cancel);


    }
}