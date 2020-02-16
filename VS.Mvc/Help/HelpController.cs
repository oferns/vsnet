namespace VS.Mvc.Help {

    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;

    [Controller]
    public class HelpController {


        [NavLink("Help", "Help", 0)]
        public IActionResult Index() => new ViewResult { ViewName = "Help" };

    }
}