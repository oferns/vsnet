namespace VS.Mvc.Home {
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Controller]
    public class HomeController {

        [HttpGet]
        public IActionResult Index() => new ViewResult { ViewName = "Home" };

    }
}
