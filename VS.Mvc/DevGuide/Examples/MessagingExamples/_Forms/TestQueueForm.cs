namespace VS.Mvc.DevGuide.Examples.MessagingExamples._forms {

    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "TestQueueForm")]
    public class TestQueueForm : LocalizedViewComponent {


        public IViewComponentResult Invoke() => View();

    }
}
