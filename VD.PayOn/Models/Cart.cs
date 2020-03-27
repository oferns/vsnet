namespace VD.PayOn.Models {
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Cart {

        [JsonPropertyName("items")]
        public IList<CartItem> Items { get; set; }

        [JsonPropertyName("payments")]
        public IList<CartPayment> Payments { get; set; }

    }
}
