
namespace VS.Abstractions.Payment {

    using System;

    public class CheckoutStatusRequest {

        public CheckoutStatusRequest(string providerIdentifier) {
            if (string.IsNullOrEmpty(providerIdentifier)) {
                throw new ArgumentException("message", nameof(providerIdentifier));
            }

            ProviderIdentifier = providerIdentifier;
        }

        public string ProviderIdentifier { get; private set; }
    }
}
