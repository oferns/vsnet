namespace VS.Mvc._Extensions {
    using Microsoft.AspNetCore.Routing;
    using System.Text.RegularExpressions;

    public class SlugifyParameterTransformer : IOutboundParameterTransformer {
        public string TransformOutbound(object value) {
            // Slugify value
            return value == null ? null : Regex.Replace(value.ToString(), "([A-Z])", "-$1", RegexOptions.Compiled).Trim('-').ToLower();
        }
    }
}
