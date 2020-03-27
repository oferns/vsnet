namespace VD.PayOn.Models {

    using System.Text.Json.Serialization;

    public class CartPayment {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public CartPaymentType PaymentType { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string CurrencyISO3 { get; set; }

        [JsonPropertyName("status")]
        public CartPaymentStatus Status { get; set; }

        [JsonPropertyName("brand")]
        public string PaymentBrand { get; set; }

        [JsonPropertyName("primary")]
        public bool PrimaryPaymentMethod { get; set; }

    }
}
