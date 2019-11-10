
namespace VS.Mvc.Error {

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    public class ErrorController : BaseController {

        public IActionResult Index(int? sc) {
            var status = sc ?? 500;

            switch (status) {
                case 500:
                default:
                return View("500");
                case 404:
                    return View("404");

        }

    }
    }
}
