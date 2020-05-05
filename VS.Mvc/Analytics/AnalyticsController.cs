namespace VS.Mvc.Analytics {

    using Microsoft.AspNetCore.Mvc;

    [Controller]
    public class AnalyticsController {

        [HttpGet]
        public IActionResult Index() {
            return new ViewResult { ViewName = "Analytics" };
        }

    }
}
