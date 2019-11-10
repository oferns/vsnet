namespace VS.Mvc.Search {
    using System;
    using Microsoft.AspNetCore.Mvc;

    [Controller]
    public class SearchController : BaseController {
        public SearchController() {
        }



        [HttpGet]       
        public IActionResult Index() {
            return View("Search");
        }
    }
}
