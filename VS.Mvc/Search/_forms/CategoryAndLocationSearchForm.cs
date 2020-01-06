namespace VS.Mvc.Search._Forms {

    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "CatAndLocSearchForm")]
    public class CategoryAndLocationSearchForm : LocalizedViewComponent {

        public IViewComponentResult Invoke() => View();

    }
}
