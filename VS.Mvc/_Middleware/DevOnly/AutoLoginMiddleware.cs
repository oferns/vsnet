
namespace VS.Mvc._Middleware.DevOnly {

    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using VS.Abstractions;
    using Microsoft.AspNetCore.Authentication;
    using System.Security.Claims;
    using System.Collections.Generic;
    using VS.Core.Identity;

    public class AutoLoginMiddleware : IMiddleware {
        private readonly IContext context;

        public AutoLoginMiddleware(IContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task InvokeAsync(HttpContext httpcontext, RequestDelegate next) {
            if (httpcontext is null) {
                throw new ArgumentNullException(nameof(httpcontext));
            }

            if (next is null) {
                throw new ArgumentNullException(nameof(next));
            }


            // httpcontext.Session.SetInt32("SomeKey", 10);
            if (context.User is object && context.User.Identity.IsAuthenticated) {
                await next(httpcontext);
                return;
            }

            var claims = new List<Claim> {
                        new Claim(IdClaimTypes.UserIdentifier, Guid.Empty.ToString(), ClaimValueTypes.String, "dev"),
                        //new Claim(IdClaimTypes.CardRef, "8ac7a4a270afe80a0170b15a7c2a7200" , ClaimValueTypes.String, "dev"),
                        //new Claim(IdClaimTypes.CardRef, "8ac7a4a170afdd600170b0407c0f106e" , ClaimValueTypes.String, "dev")
                                                                                          
                     };

            var identity = new ClaimsIdentity(claims, "DEV");
            await httpcontext.SignInAsync("DEV", new ClaimsPrincipal(identity));


            await next(httpcontext);

            

        }
    }
}
