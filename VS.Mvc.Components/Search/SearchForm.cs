namespace VS.Mvc.Components.Search {
    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;
    using VS.Mvc.Components;

    [ViewComponent(Name="SearchForm")]
    public class SearchForm : LocalizedViewComponent {


        public IViewComponentResult Invoke() {

            var query = ViewContext.RouteData.Values["q"];

            return View(new StringValues(query?.ToString() ?? string.Empty));
        }


    }
}
