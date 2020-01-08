namespace VS.Mvc.DevGuide.Examples {
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Controller]
    [Area("DevGuide")]
    public class ExamplesController {

        [HttpGet]
        public IActionResult Index() => new ViewResult { ViewName = "Examples" };
    }
}
