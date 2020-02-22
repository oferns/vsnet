namespace VS.Mvc.Payment {

    using System;
    using System.Collections.Generic;

    public class NoireFormViewModel {

        public long OrderId { get; set; }

        public Uri FormAction { get; set; }

        public Uri ReturnUrl { get; set; }

        public IEnumerable<string> CardProviders { get; set; }


    }
}