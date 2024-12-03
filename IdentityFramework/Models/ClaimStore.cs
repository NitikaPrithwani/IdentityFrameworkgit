using System.Security.Claims;

namespace IdentityFramework.Models
{
    public class ClaimStore
    {
        public static List<Claim> GetAllClaims()
        {
            return new List<Claim>()
            {
                new Claim("Create Role", "Create Role")
            };
        }
    }
}
