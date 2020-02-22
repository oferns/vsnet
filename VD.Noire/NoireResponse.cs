namespace VD.Noire {

    using System;
    using System.Text.Json.Serialization;

    public class NoireResponse {

        [JsonPropertyName("result")]
        public NoireResult Result { get; set; }

        [JsonPropertyName("buildNumber")]
        public string BuildNumber { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonPropertyName("ndc")]
        public string Ndc { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}