namespace VS.Mvc.Search {
    using System;
    using Microsoft.AspNetCore.Mvc;

    [Controller]
    public class SearchController {
        public SearchController() {
        }


        [HttpGet]       
        public IActionResult Index([FromQuery] string term) {
            return new ViewResult { ViewName = "Search" };
        }
    }
}
