namespace VS.Mvc.Search {
    using System;
    using Microsoft.AspNetCore.Mvc;

    [Controller]
    public class SearchController {
        public SearchController() {
        }


        [HttpGet]       
        public IActionResult Index() {
            return new ViewResult { ViewName = "Search" };
        }
    }
}
