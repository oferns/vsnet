namespace VS.Mvc.DevGuide.Examples {
    using Microsoft.AspNetCore.Mvc;

    [Controller]
    [Area("DevGuide")]
    public class ExamplesController {

        [HttpGet]
        public IActionResult Index() => new ViewResult { ViewName = "Examples" };
    }
}
