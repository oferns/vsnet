namespace VD.PayOn {
    using System.Text.Json.Serialization;

    public class PayOnResult {

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

    }
}
