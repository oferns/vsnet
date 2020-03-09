namespace VD.PayOn {

    using System;

    public class PayOnOptions {

        public Uri BaseUri { get; set; }

        public string Token { get; set; }

        public string EntityId { get; set; }

        public bool ExternalTestMode { get; set; }

        public bool IncludeOriginalResponse { get; set; }

    }
}
