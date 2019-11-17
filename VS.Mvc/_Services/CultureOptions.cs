namespace VS.Mvc._Services {

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public class CultureOptions {

        // Fallback Request Culture
        public CultureInfo DefaultCulture { get; set; }

        public CultureInfo DefaultUICulture { get; set; }

        public HostCultureOptions[] HostOptions { get; set; }
    }
}
