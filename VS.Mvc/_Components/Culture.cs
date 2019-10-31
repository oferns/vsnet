namespace VS.Mvc._Components {

    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using UAParser;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "CultureSwitcher")]
    public class Culture : LocalizedViewComponent {

        public IViewComponentResult Invoke() {
            var model = Parser.GetDefault().Parse(ViewContext.HttpContext.Request.Headers["User-Agent"].FirstOrDefault());
            return View(model);
        }
    }
}