namespace VS.Mvc.Error {

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [Controller]
    public class ErrorController {

        public IActionResult Index(int? sc) {
            var status = sc ?? 500;
            switch (status) {
                case 500:
                    return new ViewResult { ViewName = "500" };
                default:
                case 404:
                    return new ViewResult { ViewName = "404" };
                case 400:
                    return new BadRequestResult();
            }
        }
    }
}