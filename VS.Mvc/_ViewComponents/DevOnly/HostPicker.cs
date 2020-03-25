namespace VS.Mvc._ViewComponents.DevOnly {
    using System;
    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;
    using VS.Mvc._Services;

    public class HostPicker : LocalizedViewComponent {

        private readonly CultureOptions hostCultures;

        public HostPicker(CultureOptions hostCultures) {
            this.hostCultures = hostCultures ?? throw new ArgumentNullException(nameof(hostCultures));
        }

        public IViewComponentResult Invoke() {
            return View(this.hostCultures.HostOptions ?? Array.Empty<HostCultureOptions>());
        }
    }
}
