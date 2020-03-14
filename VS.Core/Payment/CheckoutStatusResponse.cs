namespace VS.Core.Payment {

    using System;

    public class CheckoutStatusResponse {

        public CheckoutStatusResponse(bool success, string providerReference, DateTimeOffset providerTimestamp, bool complete) {
            Success = success;
            ProviderReference = providerReference;
            ProviderTimestamp = providerTimestamp;
            Complete = complete;
        }

        public DateTimeOffset ProviderTimestamp { get; private set; }
        public bool Complete { get; }
        public bool Success { get; private set; }
        public string ProviderReference { get; private set; }

    }
}
