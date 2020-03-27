namespace VD.PayOn.Models {

    using System.Text.Json.Serialization;

    public class BankAccount {

        [JsonPropertyName("holder")]
        public string Holder { get; set; }

        [JsonPropertyName("bankName")]
        public string Bank{ get; set; }

        [JsonPropertyName("number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("iban")]
        public string IBAN { get; set; }

        [JsonPropertyName("bankCode")]
        public string BankCode { get; set; }

        [JsonPropertyName("bic")]
        public string BankIdentifierCode { get; set; }

        [JsonPropertyName("country")]
        public string CountryISO2Code { get; set; }

        [JsonPropertyName("mandate")]
        public DirectDebitMandate Mandate { get; set; }
    }
}
