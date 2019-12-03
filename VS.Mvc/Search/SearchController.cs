namespace VS.Mvc.Search {
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;

    [Controller]
    public class SearchController {
        private readonly IMediator mediator;

        public SearchController(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]       
        public async Task<IActionResult> Index([FromQuery] StringValues[] q) {

            if (q?.Length > 0) { 
                //await mediator.Send(new Searc
            
            }

            
            return new ViewResult { ViewName = "Search" };
        }
    }
}
