namespace VS.Mvc.Home {
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;

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


        [HttpGet]
        public IActionResult Component(string componentName) {
            return new ViewComponentResult() { ViewComponentName = componentName };
        }

        // This should be handled by the proxy in production
        [HttpHead]
        public IActionResult Ping() {
            return new OkResult();
        }

#if DEBUG
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Throw() => throw new ApplicationException("Test Error");
#endif


    }
}
