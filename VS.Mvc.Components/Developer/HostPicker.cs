namespace VS.Mvc.Components.Developer {

    using System;
    using Microsoft.AspNetCore.Mvc;
    using VS.Abstractions.Culture;
    using VS.Mvc.Components;

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
