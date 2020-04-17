namespace VS.Abstractions.Culture {
    using System.Collections.Generic;
    using System.Globalization;

    public class HostCultureOptions {

        public string Host { get; set; }

        public string ViewLibrary { get; set; }

        public CultureInfo DefaultCulture {get;set;}
        public CultureInfo DefaultUICulture { get; set; }

        public IEnumerable<CultureInfo> SupportedCultures { get; set; }
        public IEnumerable<CultureInfo> SupportedUICultures { get; set; }

        // We dont use the TimeZoneInfo class here because the timezone names are different across O/Ss       
        public string DefaultTimezone { get; set; }

        public IEnumerable<string> SupportedTimezones { get; set; }

    }
}
