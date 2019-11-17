namespace VS.Mvc._ViewComponents.DevOnly {

    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using UAParser.FormFactor;
    using VS.Mvc._Extensions;

    [ViewComponent(Name="UserAgentInfo")]
    public class UserAgent : LocalizedViewComponent {


        public IViewComponentResult Invoke() {
            var model = Parser.GetDefault().Parse(ViewContext.HttpContext.Request.Headers["User-Agent"].FirstOrDefault());
            return View(model);
        }
    }
}
