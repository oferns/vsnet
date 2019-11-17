namespace VS.Mvc._ViewComponents.DevOnly {
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VS.Mvc._Extensions;
    using VS.Mvc._Services;

    public class HostPicker : LocalizedViewComponent {

        private readonly CultureOptions hostCultures;

        public HostPicker(CultureOptions hostCultures) {
            this.hostCultures = hostCultures ?? throw new ArgumentNullException(nameof(hostCultures));
        }

        public IViewComponentResult Invoke() {
            return View(this.hostCultures.HostOptions ?? new HostCultureOptions[0]);
        }
    }
}
