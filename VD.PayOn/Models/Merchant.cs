namespace VD.PayOn.Models {
    using System.Text.Json.Serialization;

    public class Merchant {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("country")]
        public string CountryISO2 { get; set; }

        [JsonPropertyName("Phone")]
        public string Phone { get; set; }

        [JsonPropertyName("mcc")]
        public string MerchantCategoryCode { get; set; }

        [JsonPropertyName("submerchantId")]
        public string SubMerchantId { get; set; }
    }
}