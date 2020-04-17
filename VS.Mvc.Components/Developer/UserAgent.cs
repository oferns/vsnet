namespace VS.Mvc.Components.Developer {

    using Microsoft.AspNetCore.Mvc;
    using UAParser.FormFactor;
    using VS.Mvc.Components;

    [ViewComponent(Name="UserAgentInfo")]
    public class UserAgent : LocalizedViewComponent {


        public IViewComponentResult Invoke() {
            var model = Parser.GetDefault().Parse(ViewContext.HttpContext.Request.Headers["User-Agent"]);
            return View(model);
        }
    }
}
