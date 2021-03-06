﻿namespace VS.Mvc.Components {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc;
    using VS.Abstractions.Culture;

    [ViewComponent(Name ="LanguagePicker")]
    public class LanguagePicker : LocalizedViewComponent {

        private readonly CultureOptions hostCultures;

        public LanguagePicker(CultureOptions hostCultures) {
            this.hostCultures = hostCultures ?? throw new ArgumentNullException(nameof(hostCultures));
        }

        public IViewComponentResult Invoke() {

            var host = ViewContext.HttpContext.Request.Host.Host;

#if DEBUG
            host = ViewContext.HttpContext.Request.Cookies["vshost"]?.ToString(CultureInfo.InvariantCulture) ?? host;
#endif

          //  var cultures = new List<CultureInfo>();
            var uiCultures = new List<CultureInfo>();

            foreach (var hostOption in hostCultures.HostOptions) {
                if (hostOption.Host.Equals(host, StringComparison.OrdinalIgnoreCase)) {
                  //  cultures.AddRange(hostOption.SupportedCultures);
                    uiCultures.AddRange(hostOption.SupportedUICultures);
                }
            }

            return View(uiCultures);
        }
    }
}
