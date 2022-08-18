using System;
using System.Linq;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authorization
{
    public static class IdentityUserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                return GetClaimValue(claimsPrincipal, ClaimTypes.NameIdentifier);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static string GetUserFullName(this ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                return GetClaimValue(claimsPrincipal, ClaimTypes.GivenName);
            }
            catch (Exception)
            {
                return null;
            }

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
