using System.Linq;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authorization
{
    public static class AuthorizationPrincipalExtensions
    {
        public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
        {
            try { return GetClaimValue(claimsPrincipal, ClaimTypes.Name); }
            catch { return string.Empty; }
        }
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            try { return GetClaimValue(claimsPrincipal, ClaimTypes.NameIdentifier); }
            catch { return string.Empty; }
        }
        public static bool IsEmailConfirmed(this ClaimsPrincipal claimsPrincipal)
        {
            try { string claimValue = GetClaimValue(claimsPrincipal, "IsEmailConfirmed"); return bool.Parse(claimValue); }
            catch { return false; }
        }
        public static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            if (claimsPrincipal == null || claimsPrincipal.Claims == null)
                return null;
            var identityInfo = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType);
            if (identityInfo == null)
                return null;
            return identityInfo.Value;
        }
    }
}
