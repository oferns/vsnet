namespace VS.Mvc.Search._forms {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "CatAndLocSearchForm")]
    public class CategoryAndLocationSearchForm : LocalizedViewComponent {

        public async Task<IViewComponentResult> InvokeAsync() {

            //var categories = await med


            return View();
        } 

    }
}
