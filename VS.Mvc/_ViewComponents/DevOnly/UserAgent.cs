namespace VS.Mvc._ViewComponents.DevOnly {
    using Microsoft.AspNetCore.Mvc;
    using UAParser.FormFactor;
    using VS.Mvc._Extensions;

    [ViewComponent(Name="UserAgentInfo")]
    public class UserAgent : LocalizedViewComponent {


        public IViewComponentResult Invoke() {
            var model = Parser.GetDefault().Parse(ViewContext.HttpContext.Request.Headers["User-Agent"]);
            return View(model);
        }
    }
}
