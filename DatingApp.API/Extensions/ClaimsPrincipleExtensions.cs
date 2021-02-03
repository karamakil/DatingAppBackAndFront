using System.Security.Claims;

namespace DatingApp.API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        #region Static Methods

        public static string GetUserName(this ClaimsPrincipal userClaimPrinciple)
        {
            return userClaimPrinciple.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal userClaimPrinciple)
        {
            return int.Parse(userClaimPrinciple.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        #endregion
    }
}