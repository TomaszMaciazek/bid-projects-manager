using System.Security.Claims;
using System.Text.Json;

namespace BidProjectsManager.API.Helpers
{
    public static class RoleHelper
    {
        public static bool HasRole(this ClaimsPrincipal user ,string roleName)
        {
            return JsonSerializer.Deserialize<Dictionary<string, string[]>>(user?.FindFirst((claim) => claim?.Type == "realm_access")?.Value ?? "{}")?
                 .FirstOrDefault().Value?.Any(v => v == roleName) ?? false;
        }
    }
}
