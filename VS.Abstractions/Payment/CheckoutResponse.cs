namespace VS.Abstractions.Payment {

    using System;

    public class CheckoutResponse {

        public CheckoutResponse(bool success, string providerReference, DateTimeOffset providerTimestamp) {
            Success = success;
            ProviderReference = providerReference;
            ProviderTimestamp = providerTimestamp;
        }

        public DateTimeOffset ProviderTimestamp {get;set;}
        public bool Success { get; }
        public string ProviderReference { get; set; }

    }
}