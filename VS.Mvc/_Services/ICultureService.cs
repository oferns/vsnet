namespace VS.Mvc.Services {
    using System.Globalization;

    public interface ICultureService {

        CultureInfo[] GetSupportedUICultures(CultureInfo culture);

        CultureInfo GetDefaultUICulture(CultureInfo culture);

        
    }
}