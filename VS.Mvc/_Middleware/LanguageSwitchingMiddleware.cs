namespace VS.Mvc._Middleware {
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;
    using VS.Mvc._Extensions;

    public class LanguageSwitchingMiddleware : IMiddleware {
                          
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            if (!string.IsNullOrEmpty(context.Request.Query["uilang"].ToString())) {
                var cookieval = $"c={CultureInfo.CurrentCulture.Name}|uic={context.Request.Query["uilang"]}";
                context.Response.Cookies.Append(".AspNetCore.Culture", cookieval);
                var referer = context.Request.Headers[HeaderNames.Referer].ToString();
                var backto = context.IsLocalUrl(referer) ? referer : "/";
                backto = backto.Equals(context.Request.Path) ? "/" : backto;
                context.Response.Redirect(backto);
            } else {
                await next(context);
            }
        }
    }
}
