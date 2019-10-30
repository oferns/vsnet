
namespace VS.Mvc.Components {

    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UAParser;

    [ViewComponent(Name="UserAgentInfo")]
    public class UserAgent : ViewComponent {


        public IViewComponentResult Invoke() {
            var model = Parser.GetDefault().Parse(ViewContext.HttpContext.Request.Headers["User-Agent"].FirstOrDefault());
            return View("~/_Components/UserAgent.cshtml", model);
        }
    }
}
