namespace VS.Mvc.ClassifiedAds {
    using Microsoft.AspNetCore.Mvc;

    [Controller]
    public class ClassifiedAdsController {

        public ClassifiedAdsController() {
        }



        public IActionResult Index() => new ViewResult { ViewName = "View" };

    }
}
