using System.Security.Claims;

namespace ProfessionalPortfolio.Application.Common
{
    public static class Extensions
    {
        public static Guid GetLoggedInUserId(this ClaimsPrincipal? claimsPrincipal)
        {
            if(claimsPrincipal != null && Guid.TryParse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return userId;
            return Guid.Empty;
        }
    }
}
