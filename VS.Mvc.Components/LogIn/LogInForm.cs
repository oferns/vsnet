namespace VS.Mvc.Components.LogIn {

    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc.Components;

    [ViewComponent(Name = Name)]
    public class LogInForm : LocalizedViewComponent {

        public const string Name = "LoginForm";

        public IViewComponentResult Invoke() => View();

    }
}
