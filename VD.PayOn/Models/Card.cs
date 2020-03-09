namespace VD.PayOn.Models {

    using System.Text.Json.Serialization;

    public class CardDetails { 

        [JsonPropertyName("bin")]
        public string Id { get; set; }

        [JsonPropertyName("last4Digits")]
        public string Last4Digits { get; set; }

        [JsonPropertyName("holder")]
        public string Holder { get; set; }

        
        [JsonPropertyName("expiryMonth")]
        public string ExpiryMonth { get; set; }

        [JsonPropertyName("expiryYear")]
        public string ExpiryYear { get; set; }


    }
}
