namespace VS.Mvc._ViewComponents {

    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using UAParser;
    using VS.Mvc._Extensions;
    using VS.Mvc._Services;

    [ViewComponent(Name = "CultureSwitcher")]
    public class Culture : LocalizedViewComponent {
        private readonly HostCultureOptions[] hostCultures;

        public Culture(HostCultureOptions[] hostCultures) {
            this.hostCultures = hostCultures ?? throw new System.ArgumentNullException(nameof(hostCultures));
        }

        public IViewComponentResult Invoke() {
            return View(this.hostCultures);
        }
    }
}