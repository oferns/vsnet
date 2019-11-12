namespace VS.Mvc.Home {
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    [Controller]
    public class HomeController {
        private readonly EndpointDataSource dataSource;

        public HomeController(EndpointDataSource dataSource) {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index() => new ViewResult { ViewName = "Home" };


        [HttpGet]
        [AllowAnonymous]
        public IActionResult About() => new ViewResult { ViewName = "About" };

    }
}
