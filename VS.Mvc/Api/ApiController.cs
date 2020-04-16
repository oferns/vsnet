namespace VS.Mvc.Api {

    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Data;
    using VS.Mvc._Extensions;

    [Area("Api")]
    [DataRouteConvention]
    public class ApiController<T> : IExceptionFilter where T : class {
        
        private readonly IMediator mediator;
        private readonly IMetaData<T> metaData;

        private static readonly Dictionary<string, MethodInfo> requestCache = new Dictionary<string, MethodInfo>();
        private static object requestCacheLock = new object();


        public ApiController(IMediator mediator, IMetaData<T> metaData) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.metaData = metaData ?? throw new ArgumentNullException(nameof(metaData));
        }


        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Get(CancellationToken cancel) {

            var returnType = metaData.ReturnType;

            return new ObjectResult(returnType);

        }


        [NonAction]
        public void OnException(ExceptionContext context) {
            if (context is object) {
                context.ModelState.AddModelError("", "Oops!");
                context.Result = new ObjectResult(new SerializableError(context.ModelState)) { StatusCode = 500 };
                context.ExceptionHandled = true;
            }
        }
    }
}
