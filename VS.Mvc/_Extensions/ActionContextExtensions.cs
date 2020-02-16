namespace VS.Mvc._Extensions {
    using System;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public static class ActionContextExtensions {

        /// <summary>
        /// Will check a path string to see if it is a local url. Typically used to sanitze a return Url (ie make sure its pointing at an external site etc)
        /// </summary>
        /// <param name="context">The Action context</param>
        /// <param name="url">the url string</param>
        /// <returns>true/false</returns>
        public static bool IsLocalUrl(this HttpContext context, string url) {

            if (string.IsNullOrEmpty(url)) {
                return false;
            }

            url = url.Replace($"{context.Request.Scheme}://{context.Request.Host}", "");

            // Allows "/" or "/foo" but not "//" or "/\".
            if (url[0] == '/') {
                // url is exactly "/"
                if (url.Length == 1) {
                    return true;
                }

                // url doesn't start with "//" or "/\"
                if (url[1] != '/' && url[1] != '\\') {
                    return true;
                }

                return false;
            }

            // Allows "~/" or "~/foo" but not "~//" or "~/\".
            if (url[0] == '~' && url.Length > 1 && url[1] == '/') {
                // url is exactly "~/"
                if (url.Length == 2) {
                    return true;
                }

                // url doesn't start with "~//" or "~/\"
                if (url[2] != '/' && url[2] != '\\') {
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Signs a user in using the Framework mechanism
        /// </summary>
        /// <param name="actionContext">The Action context</param>
        /// <param name="identity">The identity to login</param>
        /// <param name="authenticationScheme">The identity scheme</param>
        /// <param name="allowRefresh">Allow refresh of cookie on subsequent requests</param>
        /// <param name="persistent">Issue a persistent cookie</param>
        /// <returns>Task</returns>
        public static async Task SignInAsync(this ActionContext actionContext, IIdentity identity, string authenticationScheme, bool allowRefresh, bool persistent) {
            var principal = new ClaimsPrincipal(identity);
            await actionContext.HttpContext.SignInAsync(
             authenticationScheme,
             principal,
             new AuthenticationProperties {
                 AllowRefresh = allowRefresh,
                 IsPersistent = persistent,
                 IssuedUtc = DateTime.UtcNow,

             });
            actionContext.HttpContext.User = principal;
        }

        /// <summary>
        /// Signs a user out using the Framework mechanism
        /// </summary>
        /// <param name="actionContext">The action context</param>
        /// <param name="authenticationScheme">The authentication scheme</param>
        /// <returns>Task</returns>
        public static async Task SignOutAsync(this ActionContext actionContext, string authenticationScheme ) {
            await actionContext.HttpContext.SignOutAsync(authenticationScheme);
        }

        /// <summary>
        /// Refreshes a Local identity when claims change mid request
        /// </summary>
        /// <param name="actionContext">The action context</param>
        /// <param name="identity"></param>
        /// <param name="authenticationScheme"></param>
        /// <returns></returns>
        public static async Task RefreshLocalIdentityAsync(this ActionContext actionContext, IIdentity identity, string authenticationScheme) {
            await actionContext.SignOutAsync(authenticationScheme);
            await actionContext.SignInAsync(identity, authenticationScheme, true, true); // TODO: Add a claim(?) saying whether they were persistent before or not
        }
    }
}