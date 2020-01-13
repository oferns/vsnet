namespace VS.Mvc.Home {
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;

    [Controller]
    public class HomeController {
        private readonly ILogger<HomeController> log;

        public HomeController(ILogger<HomeController> log) {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index() {
            return new ViewResult { ViewName = "Home" }; 
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult About() => new ViewResult { ViewName = "About" };

    }
}
