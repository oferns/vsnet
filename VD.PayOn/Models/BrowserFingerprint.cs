namespace VD.PayOn.Models {

    using System.Text.Json.Serialization;

    public class BrowserFingerprint {

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

    }
}
