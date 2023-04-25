using System.Security.Claims;

namespace Storage.App.MVC.Domain.Authorization
{
    public static class AuthorizationExtensions
    {
        public static bool ValidateUserClaims(HttpContext context,
                                                string claimName,
                                                string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                   context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }

        public static string GetClaimValue(this HttpContext context, string claimName)
        {
            if (context.User.Identity.IsAuthenticated)
                return context.User.Claims.FirstOrDefault(c => c.Type == claimName).Value;

            return null;
        }
    }
}
