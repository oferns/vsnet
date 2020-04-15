namespace VS.Mvc.LogIn._forms {

    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = LogInForm.Name)]
    public class LogInForm : LocalizedViewComponent {

        public const string Name = "LoginForm";

        public IViewComponentResult Invoke() => View();

    }
}
