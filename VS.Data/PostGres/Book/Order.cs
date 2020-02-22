namespace VS.Data.PostGres.Book {

    using System;

    public class Order {
        
        public long Id { get; set; }
        public long UserId { get; set; }
        
        public string ProviderCheckoutReference { get; set; }

        public DateTimeOffset? ProviderInitialTimestamp { get; set; }
       
    }
}
