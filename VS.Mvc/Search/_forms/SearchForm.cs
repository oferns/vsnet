namespace VS.Mvc.Search._Forms {

    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;
        
    [ViewComponent(Name="SearchForm")]
    public class SearchForm : LocalizedViewComponent {

        public IViewComponentResult Invoke() {

            var qs = ViewContext.HttpContext.Request.Query["q"];
            return View(qs);
        }


    }
}
