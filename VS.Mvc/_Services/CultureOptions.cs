﻿namespace VS.Mvc._Services {
    using System.Globalization;

    public class CultureOptions {

        // Fallback Request Culture
        public CultureInfo DefaultCulture { get; set; }

        public CultureInfo DefaultUICulture { get; set; }

        public HostCultureOptions[] HostOptions { get; set; }
    }
}
