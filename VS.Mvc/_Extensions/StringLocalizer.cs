namespace VS.Mvc._Extensions {

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.Extensions.Localization;

    public class StringLocalizer : IStringLocalizer {
        public StringLocalizer() {
        }

        public LocalizedString this[string name] {
            get {
                if (name is null) return default;
                var returnstring = name.Equals("recherche") ? "Search" : name;
                returnstring = name.Equals("Search") ? "recherche" : returnstring;

                return new LocalizedString(name, returnstring, false, string.Empty);
            }
        }

        public LocalizedString this[string name, params object[] arguments] => new LocalizedString(name, name, false, string.Empty);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) {
            return Array.Empty<LocalizedString>();
        }

        public IStringLocalizer WithCulture(CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
