
namespace VD.PayOn.Models {
    using System;
    using System.Text.Json.Serialization;

    public class DirectDebitMandate {

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("dateOfSignature")]
        public DateTimeOffset SignatureDate { get; set; }

    }
}
