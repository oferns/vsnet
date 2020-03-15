namespace VS.Mvc.Home {
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using VS.Abstractions.Logging;

    [Controller]
    public class HomeController {
        private readonly ILog log;

        public HomeController(ILog log) {
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



#if DEBUG
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Throw() => throw new ApplicationException("Test Error");
#endif


    }
}
