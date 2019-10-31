namespace VS.Mvc._Services {
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.Extensions.Primitives;

    public class HostCultureOptions {

        public string Host { get; set; }

        public CultureInfo DefaultCulture {get;set;}
        public CultureInfo DefaultUICulture { get; set; }

        public IEnumerable<CultureInfo> SupportedCultures { get; set; }
        public IEnumerable<CultureInfo> SupportedUICultures { get; set; }

    }
}
