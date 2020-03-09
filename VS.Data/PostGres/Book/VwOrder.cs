namespace VS.Data.PostGres.Book {

    using System;

    public class VwOrder {

        public VwOrder(int orderId, string host, int userId, decimal amount, string currency, decimal balance) {
            OrderId = orderId;
            Host = host;
            UserId = userId;
            Amount = amount;
            Currency = currency;
            Balance = balance;
        }

        public int OrderId { get; private set; }

        public string Host { get; private set; }

        public int UserId { get; private set; }

        public decimal Amount { get; private set; }
        
        public string Currency { get; private set; }

        public string ProviderCheckoutReference { get; private set; }
        public DateTimeOffset? ProviderInitialTimestamp { get; private set; }
    }
}