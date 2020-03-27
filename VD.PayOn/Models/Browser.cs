namespace VD.PayOn.Models {
    using System.Text.Json.Serialization;

    public class Browser {

        [JsonPropertyName("acceptHeader")]
        public string AcceptHeader { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("screenHeight")]
        public string ScreenHeight { get; set; }

        [JsonPropertyName("screenWidth")]
        public string ScreenWidth { get; set; }

        [JsonPropertyName("timezone")]
        public string TimeZoneOffset { get; set; }

        [JsonPropertyName("userAgent")]
        public string UserAgent { get; set; }

        [JsonPropertyName("javaEnabled")]
        public bool JavaEnabled { get; set; }

        [JsonPropertyName("screenColorDepth")]
        public string ScreenColorDepth { get; set; }

        [JsonPropertyName("challengeWindow")]
        public string ChallengeWindowDimensions { get; set; }

    }
}
