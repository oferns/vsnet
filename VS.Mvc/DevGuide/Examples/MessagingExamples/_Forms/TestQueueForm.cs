namespace VS.Mvc.DevGuide.Examples.MessagingExamples.Forms {

    using Microsoft.AspNetCore.Mvc;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "TestQueueForm")]
    public class TestQueueForm : LocalizedViewComponent {

        public IViewComponentResult Invoke() => View();

    }
}
