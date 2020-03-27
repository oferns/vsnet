
namespace VD.PayOn.Models {

    using System;
    using System.Text.Json.Serialization;

    public class CartItem {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("merchantItemId")]
        public string ItemId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("type")]
        public CartItemType ItemType { get; set; }

        [JsonPropertyName("sku")]
        public string StockKeepingUnit { get; set; }

        [JsonPropertyName("currency")]
        public string CurrencyISO3 { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("taxAmount")]
        public decimal TaxAmount { get; set; }

        [JsonPropertyName("totalTaxAmount")]
        public decimal TotalTaxAmount { get; set; }

        [JsonPropertyName("tax")]
        public string Tax { get; set; }

        [JsonPropertyName("shipping")]
        public decimal Shipping { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("giftMessage")]
        public string GiftMessage { get; set; }

        [JsonPropertyName("shippingMethod")]
        public string ShippingMethod { get; set; }

        [JsonPropertyName("shippingInstructions")]
        public string ShippingInstructions { get; set; }

        [JsonPropertyName("shippingTrackingNumber")]
        public string ShippingTrackingNumber { get; set; }

        [JsonPropertyName("originalPrice")]
        public string OriginalPrice { get; set; }

        [JsonPropertyName("quantityUnit")]
        public QuantityUnit QuantityUnit { get; set; }

        [JsonPropertyName("productUrl")]
        public Uri ProductUrl { get; set; }

        [JsonPropertyName("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonPropertyName("totalDiscountAmount")]
        public decimal TotalDiscountAmount { get; set; }
    }
}
