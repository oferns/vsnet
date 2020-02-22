namespace VD.Noire {

    using System.Text.Json.Serialization;

    public class NoireResult {

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
