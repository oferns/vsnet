namespace VS.Mvc.Help.DevGuide {

    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;

    [Controller]    
    public class DevGuideController {

        [HttpGet]
        [NavLink("DevGuide", "Index", 0)]
        public IActionResult Index() => new ViewResult { ViewName = "DevGuide" };

        [HttpGet]
        [NavLink("DevGuide", "Architecture", 1)]
        public IActionResult Architecture() => new ViewResult { ViewName = "Architecture" };


        [NavLink("DevGuide", "The Database", 2)]
        public IActionResult Database() => new ViewResult { ViewName = "Database" };

        [HttpGet]
        [NavLink("DevGuide", "Strategy", 3)]
        public IActionResult Strategy() => new ViewResult { ViewName = "Strategy" };

        [HttpGet]
        [NavLink("DevGuide", "Testing", 4)]
        public IActionResult Testing() => new ViewResult { ViewName = "Testing" };

        [HttpGet]
        [NavLink("DevGuide", "Data/API", 5)]
        public IActionResult Data() => new ViewResult { ViewName = "Data" };

        [HttpGet]
        [NavLink("DevGuide", "Storage", 6)]
        public IActionResult Storage() => new ViewResult { ViewName = "Storage" };

        [HttpGet]
        [NavLink("DevGuide", "Logging", 7)]
        public IActionResult Logging() => new ViewResult { ViewName = "Logging" };

        [HttpGet]
        [NavLink("DevGuide", "Authentication", 8)]
        public IActionResult Authentication() => new ViewResult { ViewName = "Authentication" };

        [HttpGet]
        [NavLink("DevGuide", "Authorization", 9)]
        public IActionResult Authorization() => new ViewResult { ViewName = "Authorization" };

    }
}
