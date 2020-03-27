namespace VD.PayOn.Models {

    using System.Text.Json.Serialization;

    public class Passenger {

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ticketRestricted")]
        public string Restricted { get; set; }

        [JsonPropertyName("ticketNumber")]
        public string TicketNumber { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("frequentFlyerNumber")]
        public string FrequentFlyerNumber { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("dob")]
        public string Dob { get; set; }

        [JsonPropertyName("checkDigit")]
        public string CheckDigit { get; set; }


    }
}
