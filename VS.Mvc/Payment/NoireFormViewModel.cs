namespace VS.Mvc.Payment {

    using System;
    using System.Collections.Generic;

    public class NoireFormViewModel {

        public Uri FormAction { get; set; }

        public Uri ReturnUrl { get; set; }

        public IEnumerable<string> CardProviders { get; set; }
    }
}