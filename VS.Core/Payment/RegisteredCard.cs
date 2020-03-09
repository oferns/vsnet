namespace VS.Core.Payment {

    public class RegisteredCard {

        public RegisteredCard(string cardHolder, string last4Digits, string cardProvider, string expiry, string providerReference) {
            CardHolder = cardHolder;
            Last4Digits = last4Digits;
            CardProvider = cardProvider;
            Expiry = expiry;
            ProviderReference = providerReference;
        }

        public string CardHolder { get; set; }
        
        public string Last4Digits { get; private set; }

        public string CardProvider { get; private set; }

        public string Expiry { get; private set; }
        public string ProviderReference { get; private set; }
    }
    
}