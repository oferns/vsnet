namespace VS.Mvc.Search {
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;

    [Controller]
    public class SearchController {
    
        [HttpGet]
        public IActionResult Index() {

            return new ViewResult { ViewName = "Search" };
        }
    }
}
