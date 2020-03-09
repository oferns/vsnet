namespace VS.Core.Identity {

    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions {


        public static void AddClaim(this ClaimsPrincipal principal, Claim claim) {
            if (principal.Identity is ClaimsIdentity claimsId) {
                claimsId.AddClaim(claim);
            }
        }
    }
}
