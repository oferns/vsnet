namespace VS.Mvc._Middleware {
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class ReExecuteOnValidationErrorMiddleware : IMiddleware {
        public Task InvokeAsync(HttpContext context, RequestDelegate next) {
            throw new NotImplementedException();
        }
    }
}
