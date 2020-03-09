namespace VS.Core.Payment {

    using System;

    public class RegisterCardStatusResponse {


        public RegisterCardStatusResponse(bool success, RegisteredCard card, string providerReference, Uri location, DateTimeOffset providerTimestamp) {
            Success = success;
            Card = card;
            ProviderReference = providerReference;
            Location = location;
            ProviderTimestamp = providerTimestamp;
        }

        public DateTimeOffset ProviderTimestamp { get; private set; }
        public bool Success { get; private set; }
        public RegisteredCard Card { get; private set; }
        public string ProviderReference { get; private set; }
        public Uri Location { get; private set; }
    }
}