namespace VD.PayOn.Models {

    using System;
    using System.Text.Json.Serialization;

    public class Customer {

        [JsonPropertyName("merchantCustomerId")]
        public string MerchantCustomerId { get; set; }

        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("middleName")]
        public string MiddleName { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("sex")]
        public char Sex { get; set; }

        [JsonPropertyName("birthDate")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("workPhone")]
        public string WorkPhone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("companyName")]
        public string Company { get; set; }

        [JsonPropertyName("identificationDocType")]
        public string IdDocumentType { get; set; }

        [JsonPropertyName("identificationDocId")]
        public string IdDocumentId { get; set; }

        [JsonPropertyName("ip")]
        public string IPAddress { get; set; }

        [JsonPropertyName("browserFingerprint")]
        public BrowserFingerprint Fingerprint { get; set; }

        [JsonPropertyName("browser")]
        public Browser Browser { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

    }
}
