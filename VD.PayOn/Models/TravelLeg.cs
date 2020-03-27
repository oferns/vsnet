namespace VD.PayOn.Models {
    using System;
    using System.Text.Json.Serialization;

    public class TravelLeg {

        [JsonPropertyName("ticketNumber")]
        public string TicketNumber { get; set; }

        [JsonPropertyName("taxAmount")]
        public decimal Tax { get; set; }

        [JsonPropertyName("stopOverAllowed")]
        public string StopOverAllowed { get; set; }

        [JsonPropertyName("restrictions")]
        public string Restrictions { get; set; }

        [JsonPropertyName("flightNumber")]
        public string FlightNumber { get; set; }

        [JsonPropertyName("feesAmount")]
        public decimal Fees { get; set; }

        [JsonPropertyName("fareBasis")]
        public string FareBasis { get; set; }

        [JsonPropertyName("fareAmount")]
        public decimal Fare { get; set; }

        [JsonPropertyName("exchangeTicketNum")]
        public string ExchangeTicketNum { get; set; }

        [JsonPropertyName("departureTaxAmount")]
        public decimal DepartureTax { get; set; }

        [JsonPropertyName("departureCountry")]
        public string DepartureCountry { get; set; }

        [JsonPropertyName("departureAirport")]
        public string DepartureAirport { get; set; }

        [JsonPropertyName("airlineName")]
        public string Airline { get; set; }

        [JsonPropertyName("airlineCode")]
        public string AirlineCode { get; set; }

        [JsonPropertyName("departureTime")]
        public string DepartureTime { get; set; }

        [JsonPropertyName("departureDate")]
        public DateTime DepartureDate { get; set; }

        [JsonPropertyName("arrivalCountry")]
        public string ArrivalCountry { get; set; }

        [JsonPropertyName("arrivalCountry")]
        public string ArrivalAirport { get; set; }

        [JsonPropertyName("arrivalTime")]
        public string ArrivalTime { get; set; }

        [JsonPropertyName("arrivalDate")]
        public DateTime ArrivalDate { get; set; }

        [JsonPropertyName("couponNumber")]
        public string CouponNumber { get; set; }

        [JsonPropertyName("classOfService")]
        public string Class { get; set; }

        [JsonPropertyName("carrierCode")]
        public string CarrierCode { get; set; }


    }
}
