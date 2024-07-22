using System.Security.Claims;

namespace RunWebAppGroup.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {

            return user.FindFirst(ClaimTypes.Name).Value;
        }
    }
}