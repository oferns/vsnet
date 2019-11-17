namespace VS.Mvc._ViewComponents {

    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using VS.Mvc._Extensions;
    using VS.Mvc._Services;

    [ViewComponent(Name ="LanguagePicker")]
    public class LanguagePicker : LocalizedViewComponent {

        private readonly CultureOptions hostCultures;

        public LanguagePicker(CultureOptions hostCultures) {
            this.hostCultures = hostCultures ?? throw new ArgumentNullException(nameof(hostCultures));
        }

        public IViewComponentResult Invoke() {

            var host = ViewContext.HttpContext.Request.Host.Host;

#if DEBUG
            host = ViewContext.HttpContext.Request.Cookies["vshost"]?.ToString() ?? host;
#endif

            var uicultures = hostCultures.HostOptions.Where(c => c.Host.Equals(host)).SelectMany(c => c.SupportedUICultures);

            return View(uicultures?.ToArray());
        }
    }
}
