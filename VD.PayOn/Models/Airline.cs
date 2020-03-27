namespace VD.PayOn.Models {
    using System;
    using System.Text.Json.Serialization;

    public class Airline {

        [JsonPropertyName("totalTaxAmount")]
        public double TotalTax { get; set; }

        [JsonPropertyName("totalFeesAmount")]
        public double TotalFees { get; set; }

        [JsonPropertyName("totalFareAmount")]
        public double TotalFare { get; set; }

        [JsonPropertyName("ticketIssueDate")]
        public DateTime IssueDate { get; set; }

        [JsonPropertyName("ticketIssueAddress")]
        public string IssueAddress { get; set; }

        [JsonPropertyName("thirdPartyBooking")]
        public bool ThirdPartyBooking { get; set; }

        [JsonPropertyName("bookingtype")]
        public string Bookingtype { get; set; }

        [JsonPropertyName("ticketDeliveryMethod")]
        public string DeliveryMethod { get; set; }

        [JsonPropertyName("bookingRefNum")]
        public string BookingReference { get; set; }

        [JsonPropertyName("agentName")]
        public string AgentName { get; set; }
        
        [JsonPropertyName("agentCode")]
        public string AgentCode { get; set; }



    }
}
