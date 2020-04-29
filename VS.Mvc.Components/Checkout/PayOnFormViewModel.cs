namespace VS.Mvc.Components.Checkout {

    using System;
    using System.Collections.Generic;
    using VS.Core.Payment;

    public class PayOnFormViewModel {

        public long OrderId { get; set; }

        public string OppWaId { get; set; }

        public IList<RegisteredCard> Cards { get; set; }

        public Uri CardFormAction { get; set; }

        public Uri FormAction { get; set; }

        public Uri ReturnUrl { get; set; }

        public IEnumerable<string> CardProviders { get; set; }


    }
}