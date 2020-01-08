
namespace VS.Mvc.DevGuide.Examples.Messaging._forms {
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VS.Mvc._Extensions;

    [ViewComponent(Name = "TestQueueForm")]
    public class TestQueueForm : LocalizedViewComponent {


        public IViewComponentResult Invoke() => View();

    }
}
