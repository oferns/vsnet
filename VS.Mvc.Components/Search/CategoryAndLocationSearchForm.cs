namespace VS.Mvc.Components.Search {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc.Components;

    [ViewComponent(Name = "CatAndLocSearchForm")]
    public class CategoryAndLocationSearchForm : LocalizedViewComponent {

        public async Task<IViewComponentResult> InvokeAsync() {

            //var categories = await med


            return View();
        } 

    }
}
