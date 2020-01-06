namespace VS.Mvc.Search {
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Primitives;
    using VS.Core.Search;

    [Controller]
    public class SearchController : ControllerBase {
        
        private readonly IMediator mediator;
        private ViewDataDictionary viewData;

        public SearchController(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]       
        public async Task<IActionResult> Index([FromQuery] StringValues[] q) {

            if (q?.Length > 0) {
                viewData.Model = await mediator.Send(new SearchRequest(q));                             
            }
            
            return new ViewResult { ViewName = "Search", ViewData = viewData };
        }

        /// <summary>
        /// Gets or sets <see cref="ViewDataDictionary"/> used by <see cref="ViewResult"/> and <see cref="ViewBag"/>.
        /// </summary>
        /// <remarks>
        /// By default, this property is intiailized when <see cref="Controllers.IControllerActivator"/> activates
        /// controllers.
        /// <para>
        /// This property can be accessed after the controller has been activated, for example, in a controller action
        /// or by overriding <see cref="OnActionExecuting(ActionExecutingContext)"/>.
        /// </para>
        /// <para>
        /// This property can be also accessed from within a unit test where it is initialized with
        /// <see cref="EmptyModelMetadataProvider"/>.
        /// </para>
        /// </remarks>
        [ViewDataDictionary]
        public ViewDataDictionary ViewData {
            get {
                return viewData;
            }
            set => viewData = value ?? throw new ArgumentException("ViewDataDictionary", nameof(ViewData));
        }
    }
}
