namespace VS.Core.Payment {

    using System;

    public class ReqisterCardResponse {

        public ReqisterCardResponse(bool success, string providerReference, DateTimeOffset providerTimestamp) {
            Success = success;
            ProviderReference = providerReference;
            ProviderTimestamp = providerTimestamp;
        }

        public DateTimeOffset ProviderTimestamp { get; private set; }
        public bool Success { get; private set; }
        public string ProviderReference { get; private set; }

    }
}
