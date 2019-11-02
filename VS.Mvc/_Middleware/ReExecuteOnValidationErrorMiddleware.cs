using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VS.Mvc._Middleware {
    public class ReExecuteOnValidationErrorMiddleware : IMiddleware {
        public Task InvokeAsync(HttpContext context, RequestDelegate next) {
            throw new NotImplementedException();
        }
    }
}
