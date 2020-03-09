namespace VD.PayOn {

    using VD.PayOn.Models;
    using System.Text.Json.Serialization;
    using System.Collections.Generic;

    public class RegisterCardResponse : PayOnResponse {

        [JsonPropertyName("card")]
        public CardDetails Card { get; set; }
        
        [JsonPropertyName("paymentBrand")]
        public string Brand { get; set; }

        [JsonPropertyName("customParameters")]
        public IDictionary<string, object> CustomParameters { get; set; }

        [JsonPropertyName("risk")]
        public Risk Risk { get; set; }
    }
}
