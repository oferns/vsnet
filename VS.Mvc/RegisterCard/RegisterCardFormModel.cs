namespace VS.Mvc.RegisterCard {

    using System;
    using System.Collections.Generic;

    public class RegisterCardFormModel {

        public string OppWaId { get; set; }

        public Uri CardFormAction { get; set; }


        public Uri FormAction { get; set; }

        public Uri ReturnUrl { get; set; }

        public IEnumerable<string> CardProviders { get; set; }

    }
}
