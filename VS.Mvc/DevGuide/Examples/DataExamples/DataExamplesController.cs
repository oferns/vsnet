namespace VS.Mvc.DevGuide.Examples.Data {

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using VS.Mvc._Extensions;

    [Area("DevGuide")]
    [SubArea("Examples")]
    [Controller]
    public class DataExamplesController {
        private readonly IActionContextAccessor context;

        public DataExamplesController(IActionContextAccessor context) {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult Index() => new ViewResult { ViewName = "DataExamples" };

        [HttpGet]
        public IActionResult Item([FromRoute] int eid) {
            context.ActionContext.RouteData.Values.Add("test_id", eid);
            return new ViewResult { ViewName = "Edit" };
        }
    }
}
