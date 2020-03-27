namespace VD.PayOn.Models {
    using System.Text.Json.Serialization;

    public class Address {

        [JsonPropertyName("street1")]
        public string Street1 { get; set; }

        [JsonPropertyName("street2")]
        public string Street2 { get; set; }

        [JsonPropertyName("houseNumber1")]
        public string HouseNumber1 { get; set; }

        [JsonPropertyName("houseNumber2")]
        public string HouseNumber2 { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        [JsonPropertyName("country")]
        public string CountryISO2 { get; set; }

        [JsonPropertyName("normalized")]
        public string NormalizedAddress { get; set; }

        [JsonPropertyName("validationStatus")]
        public string ValidationStatus { get; set; }

    }
}
