namespace VS.Data.PostGres.Book {

    using System;

    public class OrderItem {


        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Host { get; set; }
        public string ItemPath { get; set; }
        public int PlanId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public TimeSpan Lifetime { get; set; }
        public DateTimeOffset Created { get; set; }

    }
}
