
namespace VD.PayOn.Models {

    using System;
    using System.Text.Json.Serialization;

    public class ShippingInfo : Address {


        [JsonPropertyName("method")]
        public ShippingMethod ShippingMethod { get; set; }

        [JsonPropertyName("cost")]
        public decimal Cost { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("expectedDate")]
        public DateTime ExpectedDeliveryDate { get; set; }

        [JsonPropertyName("logisticsProvider")]
        public string LogisticsProvider { get; set; }

        [JsonPropertyName("trackingNumber")]
        public string TrackingNumber { get; set; }

        [JsonPropertyName("returnTrackingNumber")]
        public string ReturnTrackingNumber { get; set; }

        [JsonPropertyName("warehouse")]
        public string Warehouse { get; set; }

    }
}
