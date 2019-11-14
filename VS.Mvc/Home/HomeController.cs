namespace VS.Mvc.Home {
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    [Controller]
    public class HomeController {


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
